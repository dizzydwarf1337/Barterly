using System.Linq.Expressions;
using Application.Core.ApiResponse;
using Application.DTOs.Posts;
using AutoMapper;
using Domain.Entities.Posts;
using Domain.Entities.Posts.PostTypes;
using Domain.Enums.Posts;
using Domain.Interfaces.Queries.Post;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Public.Posts.GetPostsFiltredPaginated;

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
        if (request.FilterBy?.PageSize <= 0 || request.FilterBy?.PageNumber <= 0)
            return ApiResponse<GetPostsQuery.Result>.Failure("Invalid pagination parameters.");

        var promotedPostsPerPage = (int)(request.FilterBy.PageSize / 3.0);
        
        var promotedPosts = await GetPromotedPosts(promotedPostsPerPage, request.FilterBy.PageNumber, 
            cancellationToken);
        
        var regularPostsPerPage = request.FilterBy.PageSize - promotedPosts.Count;
        var regularPosts = await GetRegularPosts(regularPostsPerPage, request.FilterBy.PageNumber, 
            request.FilterBy, request.SortBy, cancellationToken);

        var shuffledPosts = ShufflePosts(regularPosts, promotedPosts);

        var totalRegularCount = await GetTotalRegularCount(request.FilterBy, cancellationToken);
        var totalPromotedCount = await GetTotalPromotedCount(cancellationToken);
        var totalCount = totalRegularCount + totalPromotedCount;
        var totalPages = (int)Math.Ceiling(totalCount / (double)request.FilterBy.PageSize);

        return ApiResponse<GetPostsQuery.Result>.Success(new GetPostsQuery.Result
        {
            Items = shuffledPosts,
            TotalCount = totalCount,
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
            .OrderBy(x => x.ViewsCount)
            .ThenBy(x => Guid.NewGuid())
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
            .OrderBy(x => x.ViewsCount)
            .ThenBy(x => Guid.NewGuid())
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

    private async Task<int> GetTotalPromotedCount(CancellationToken cancellationToken)
    {
        return await _postQueryRepository.GetAllPosts()
            .Where(x =>
                x.PostSettings.postStatusType == PostStatusType.Published &&
                !x.PostSettings.IsDeleted &&
                !x.PostSettings.IsHidden &&
                x.Promotion.Type != PostPromotionType.None)
            .CountAsync(cancellationToken);
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
        switch (filter.PostType)
        {
            case "Work":
                yield return p =>
                    p is WorkPost &&
                    (!filter.Workload.HasValue || ((WorkPost)p).Workload == filter.Workload);
                yield return p =>
                    p is WorkPost &&
                    (!filter.WorkLocation.HasValue || ((WorkPost)p).WorkLocation == filter.WorkLocation);
                yield return p =>
                    p is WorkPost &&
                    (!filter.MinSalary.HasValue || ((WorkPost)p).MinSalary >= filter.MinSalary);
                yield return p =>
                    p is WorkPost &&
                    (!filter.MaxSalary.HasValue || ((WorkPost)p).MaxSalary <= filter.MaxSalary);
                yield return p =>
                    p is WorkPost &&
                    (!filter.ExperienceRequired.HasValue || ((WorkPost)p).ExperienceRequired == filter.ExperienceRequired);
                break;
            case "Rent":
                yield return p =>
                    p is RentPost &&
                    (!filter.RentObjectType.HasValue || ((RentPost)p).RentObjectType == filter.RentObjectType);
                yield return p => 
                    p is RentPost && 
                    (!filter.NumberOfRooms.HasValue || ((RentPost)p).NumberOfRooms == filter.NumberOfRooms);
                yield return p => 
                    p is RentPost &&
                    (!filter.Area.HasValue || ((RentPost)p).Area == filter.Area);
                yield return p => 
                    p is RentPost &&
                    (!filter.Floor.HasValue || ((RentPost)p).Floor == filter.Floor);
                break;
        }
        
        if (!string.IsNullOrWhiteSpace(filter.Search))
            yield return p => p.Title.ToLower().Contains(filter.Search.ToLower());

        if (filter.CategoryId.HasValue)
        {
            if (filter.SubCategoryId.HasValue)
                yield return p => p.SubCategoryId == filter.SubCategoryId.Value;
            yield return p => p.SubCategory.CategoryId == filter.CategoryId.Value;
        }
        
        if (!string.IsNullOrWhiteSpace(filter.City))
        {
            var cityLower = filter.City.ToLower();
            yield return p => (p.City ?? "").ToLower().Contains(cityLower);
        }
        
        if (filter.MinPrice.HasValue)
            yield return p => p.Price >= filter.MinPrice.Value;
        if (filter.MaxPrice.HasValue)
            yield return p => p.Price <= filter.MaxPrice;
        if (filter.SubCategoryId.HasValue)
            yield return p => p.SubCategoryId == filter.SubCategoryId.Value;
    }
}