using Application.Core.ApiResponse;
using Application.DTOs.Posts;
using AutoMapper;
using Domain.Enums.Posts;
using Domain.Interfaces.Queries.Post;
using Domain.Interfaces.Queries.User;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Users.Posts.GetFeed;

public class GetFeedQueryHandler : IRequestHandler<GetFeedQuery, ApiResponse<GetFeedQuery.Result>>
{
    private readonly IPostQueryRepository _postQueryRepository;
    private readonly IUserActivityQueryRepository _userActivityQueryRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetFeedQueryHandler> _logger;

    private const int MinimumPostsCount = 7;

    public GetFeedQueryHandler(
        IPostQueryRepository postQueryRepository, 
        IUserActivityQueryRepository userActivityQueryRepository, 
        IMapper mapper,
        ILogger<GetFeedQueryHandler> logger)
    { 
        _postQueryRepository = postQueryRepository;
        _userActivityQueryRepository = userActivityQueryRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApiResponse<GetFeedQuery.Result>> Handle(
        GetFeedQuery request,
        CancellationToken cancellationToken)
    {
        var userActivity = await _userActivityQueryRepository
            .GetUserActivityByUserIdAsync(request.AuthorizeData!.UserId, cancellationToken);

        var allPreferredCities = ParsePreferences(userActivity?.MostViewedCities);
        var preferredCategories = ParsePreferences(userActivity?.MostViewedCategories);
        
        var preferredCities = allPreferredCities.Count >= 2 
            ? allPreferredCities.OrderBy(_ => Random.Shared.Next()).Take(2).ToHashSet(StringComparer.OrdinalIgnoreCase)
            : allPreferredCities;
    
        _logger.LogInformation("Selected random cities for feed: {Cities}", string.Join(", ", preferredCities));
    
        var pageSize = request.FilterBy?.PageSize ?? 10;
        var pageNumber = request.FilterBy?.PageNumber ?? 1;

        var feedPosts = await GetPersonalizedFeed(
            preferredCities, 
            preferredCategories,
            cancellationToken);
    
        var totalCount = 7;

        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var postsDto = _mapper.Map<List<PostPreviewDto>>(feedPosts);

        return ApiResponse<GetFeedQuery.Result>.Success(new GetFeedQuery.Result
        {
            Items = postsDto,
            TotalCount = totalCount,
            TotalPages = totalPages
        });
    }

    private async Task<List<Domain.Entities.Posts.Post>> GetPersonalizedFeed(
    HashSet<string> preferredCities,
    HashSet<string> preferredCategories,
    CancellationToken cancellationToken)
{
    var baseQuery = _postQueryRepository.GetAllPosts()
        .Include(x => x.SubCategory)
            .ThenInclude(sc => sc.Category)
        .Include(x => x.Owner)
        .Where(p => 
            p.PostSettings.postStatusType == PostStatusType.Published &&
            !p.PostSettings.IsDeleted &&
            !p.PostSettings.IsHidden);
    
    var cityAndCategoryPosts = await baseQuery
        .Where(p => 
            p.City != null && preferredCities.Contains(p.City) &&
            p.SubCategory.Category.NameEN != null && preferredCategories.Contains(p.SubCategory.Category.NameEN))
        .OrderByDescending(p => p.ViewsCount)
        .ThenByDescending(p => p.CreatedAt)
        .Take(3) 
        .ToListAsync(cancellationToken);

    var usedIds = cityAndCategoryPosts.Select(p => p.Id).ToHashSet();

    var cityOnlyPosts = await baseQuery
        .Where(p => 
            !usedIds.Contains(p.Id) &&
            p.City != null && preferredCities.Contains(p.City))
        .OrderByDescending(p => p.ViewsCount)
        .ThenByDescending(p => p.CreatedAt)
        .Take(3)
        .ToListAsync(cancellationToken);

    usedIds.UnionWith(cityOnlyPosts.Select(p => p.Id));

    var categoryOnlyPosts = await baseQuery
        .Where(p => 
            !usedIds.Contains(p.Id) &&
            p.SubCategory.Category.NameEN != null && preferredCategories.Contains(p.SubCategory.Category.NameEN))
        .OrderByDescending(p => p.ViewsCount)
        .ThenByDescending(p => p.CreatedAt)
        .Take(1)
        .ToListAsync(cancellationToken);

    var personalizedPosts = new List<Domain.Entities.Posts.Post>();
    personalizedPosts.AddRange(cityAndCategoryPosts);
    personalizedPosts.AddRange(cityOnlyPosts);
    personalizedPosts.AddRange(categoryOnlyPosts);

    if (personalizedPosts.Count < MinimumPostsCount)
    {
        usedIds.UnionWith(categoryOnlyPosts.Select(p => p.Id));
        var additionalCount = MinimumPostsCount - personalizedPosts.Count;

        var generalPosts = await baseQuery
            .Where(p => !usedIds.Contains(p.Id))
            .OrderByDescending(p => p.ViewsCount)
            .ThenByDescending(p => p.CreatedAt)
            .Take(additionalCount)
            .ToListAsync(cancellationToken);

        personalizedPosts.AddRange(generalPosts);
    }

    return personalizedPosts;
}

    private static HashSet<string> ParsePreferences(string? preferencesString)
    {
        if (string.IsNullOrWhiteSpace(preferencesString))
            return new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        return preferencesString
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
    }

    private static bool IsPersonalized(
        Domain.Entities.Posts.Post post, 
        HashSet<string> preferredCities, 
        HashSet<string> preferredCategories)
    {
        return (post.City != null && preferredCities.Contains(post.City)) ||
               (post.SubCategory?.Category?.NameEN != null && preferredCategories.Contains(post.SubCategory.Category.NameEN));
    }
}