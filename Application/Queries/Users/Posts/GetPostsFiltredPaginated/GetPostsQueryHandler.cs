using System.Linq.Expressions;
using Application.Core.ApiResponse;
using Application.DTOs.Posts;
using AutoMapper;
using Domain.Entities.Posts;
using Domain.Enums.Posts;
using Domain.Interfaces.Queries.Post;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Users.Posts.GetPostsFiltredPaginated;

public class GetPostsQueryHandler : IRequestHandler<GetPostsQuery, ApiResponse<GetPostsQuery.Result>>
{
    private readonly IMapper _mapper;
    private readonly IPostQueryRepository _postQueryRepository;

    public GetPostsQueryHandler(IPostQueryRepository postQueryRepository, IMapper mapper)
    {
        _postQueryRepository = postQueryRepository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<GetPostsQuery.Result>> Handle(GetPostsQuery request,
        CancellationToken cancellationToken)
    {
        var regularPostsPerPage = request.FilterBy!.PageSize - (int)(request.FilterBy.PageSize / 3.0);
        var promotedPostsPerPage = (int)(request.FilterBy.PageSize / 3.0);

        var regularPosts = await GetRegularPosts(regularPostsPerPage, request.FilterBy.PageNumber, 
            request.FilterBy, request.SortBy, cancellationToken);

        var promotedPosts = await GetPromotedPosts(promotedPostsPerPage, request.FilterBy.PageNumber, 
            cancellationToken);

        var shuffledPosts = ShufflePosts(regularPosts, promotedPosts);

        var totalRegularCount = await GetTotalRegularCount(request.FilterBy, cancellationToken);
        var totalPages = (int)Math.Ceiling(totalRegularCount / (double)regularPostsPerPage);

        return ApiResponse<GetPostsQuery.Result>.Success(new GetPostsQuery.Result
        {
            Items = shuffledPosts,
            TotalCount = totalRegularCount,
            TotalPages = totalPages
        });
    }

    private async Task<ICollection<Post>> GetRegularPosts(int pageSize, int pageNumber, 
        GetPostsQuery.FilterSpecification filter, GetPostsQuery.SortSpecification? sortBy, 
        CancellationToken cancellationToken)
    {
        var query = _postQueryRepository.GetAllPosts()
            .Include(x => x.Owner)
            .Where(x =>
                x.PostSettings.postStatusType == PostStatusType.Published &&
                !x.PostSettings.IsDeleted &&
                !x.PostSettings.IsHidden &&
                x.Promotion.Type == PostPromotionType.None);

        foreach (var filterExpression in GetFilters(filter))
        {
            query = query.Where(filterExpression);
        }

        if (sortBy != null && !string.IsNullOrWhiteSpace(sortBy.SortBy))
        {
            var sortField = sortBy.SortBy.ToLower();
            query = sortField switch
            {
                "title" => sortBy.IsDescending ? query.OrderByDescending(p => p.Title) : query.OrderBy(p => p.Title),
                "createdat" => sortBy.IsDescending ? query.OrderByDescending(p => p.CreatedAt) : query.OrderBy(p => p.CreatedAt),
                "viewscount" => sortBy.IsDescending ? query.OrderByDescending(p => p.ViewsCount) : query.OrderBy(p => p.ViewsCount),
                _ => query.OrderByDescending(p => p.CreatedAt)
            };
        }
        else
        {
            query = query.OrderByDescending(p => p.CreatedAt);
        }

        query = query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        return await query.ToListAsync(cancellationToken);
    }

    private async Task<ICollection<Post>> GetPromotedPosts(int count, int pageNumber, 
        CancellationToken cancellationToken)
    {
        var topPostsCount = (int)Math.Ceiling(count * 2 / 3.0);
        var highlightPostsCount = count - topPostsCount;

        var topPosts = await _postQueryRepository.GetAllPosts()
            .Include(x => x.Owner)
            .Where(x =>
                x.Promotion.Type == PostPromotionType.Top &&
                x.PostSettings.postStatusType == PostStatusType.Published &&
                !x.PostSettings.IsDeleted &&
                !x.PostSettings.IsHidden)
            .OrderByDescending(x => x.Id)
            .Skip((pageNumber - 1) * topPostsCount)
            .Take(topPostsCount)
            .ToListAsync(cancellationToken);

        var highlightPosts = await _postQueryRepository.GetAllPosts()
            .Include(x => x.Owner)
            .Where(x =>
                x.Promotion.Type == PostPromotionType.Highlight &&
                x.PostSettings.postStatusType == PostStatusType.Published &&
                !x.PostSettings.IsDeleted &&
                !x.PostSettings.IsHidden)
            .OrderByDescending(x => x.ViewsCount)
            .Skip((pageNumber - 1) * highlightPostsCount)
            .Take(highlightPostsCount)
            .ToListAsync(cancellationToken);

        return topPosts.Concat(highlightPosts).ToList();
    }

    private async Task<int> GetTotalRegularCount(GetPostsQuery.FilterSpecification filter, 
        CancellationToken cancellationToken)
    {
        var query = _postQueryRepository.GetAllPosts()
            .Where(x =>
                x.PostSettings.postStatusType == PostStatusType.Published &&
                !x.PostSettings.IsDeleted &&
                !x.PostSettings.IsHidden &&
                x.Promotion.Type == PostPromotionType.None);

        foreach (var filterExpression in GetFilters(filter))
        {
            query = query.Where(filterExpression);
        }

        return await query.CountAsync(cancellationToken);
    }

    private List<PostPreviewDto> ShufflePosts(ICollection<Post> regularPosts, ICollection<Post> promotedPosts)
    {
        var rnd = new Random();

        var shuffledRegular = regularPosts.OrderBy(_ => rnd.Next()).ToList();
        var shuffledPromoted = promotedPosts.OrderBy(_ => rnd.Next()).ToList();

        var total = shuffledRegular.Count + shuffledPromoted.Count;
        var result = new List<Post>(total);

        var regularIndex = 0;
        var promotedIndex = 0;

        var regularRatio = shuffledRegular.Count > 0 ? shuffledRegular.Count / (double)total : 0;
        var promotedRatio = shuffledPromoted.Count > 0 ? shuffledPromoted.Count / (double)total : 0;

        double regularCounter = 0;
        double promotedCounter = 0;

        for (var i = 0; i < total; i++)
        {
            var pickPromoted =
                promotedIndex < shuffledPromoted.Count &&
                (regularIndex >= shuffledRegular.Count || promotedCounter <= regularCounter);

            if (pickPromoted)
            {
                result.Add(shuffledPromoted[promotedIndex++]);
                if (promotedRatio > 0)
                    promotedCounter += 1 / promotedRatio;
            }
            else
            {
                result.Add(shuffledRegular[regularIndex++]);
                if (regularRatio > 0)
                    regularCounter += 1 / regularRatio;
            }
        }

        return _mapper.Map<List<PostPreviewDto>>(result);
    }

    private IEnumerable<Expression<Func<Post, bool>>> GetFilters(GetPostsQuery.FilterSpecification filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Search))
            yield return p => p.Title.ToLower().Contains(filter.Search.ToLower());

        if (filter.SubCategoryId.HasValue)
            yield return p => p.SubCategoryId == filter.SubCategoryId.Value;
        
        if(!string.IsNullOrWhiteSpace(filter.City))
            yield return p => p.City.ToLower().Contains(filter.City.ToLower());

        if (filter.UserId.HasValue)
            yield return p => p.OwnerId == filter.UserId.Value;
    }
}