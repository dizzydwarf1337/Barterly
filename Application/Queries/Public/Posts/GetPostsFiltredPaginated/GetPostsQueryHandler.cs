using System.Linq.Expressions;
using System.Text;
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
        var posts = _postQueryRepository.GetAllPosts().Where(x=> !x.PostSettings.IsDeleted && !x.PostSettings.IsHidden && x.PostSettings.postStatusType == PostStatusType.Published);
        if (request.FilterBy?.PageSize <= 0 || request.FilterBy?.PageNumber <= 0)
            return ApiResponse<GetPostsQuery.Result>.Failure("Invalid pagination parameters.");

        foreach (var filter in GetFilters(request.FilterBy)) posts = posts.Where(filter);

        if (!string.IsNullOrWhiteSpace(request.SortBy?.SortBy))
        {
            var sortField = request.SortBy.SortBy.ToLower();
            var descending = request.SortBy.IsDescending;

            posts = sortField switch
            {
                "title" => descending ? posts.OrderByDescending(p => p.Title) : posts.OrderBy(p => p.Title),
                "createdat" => descending
                    ? posts.OrderByDescending(p => p.CreatedAt)
                    : posts.OrderBy(p => p.CreatedAt),
                _ => posts
            };
        }

        var totalCount = posts.Count();
        var totalPages = (int)Math.Ceiling(totalCount / (double)request.FilterBy.PageSize);
        var items = _mapper.Map<List<PostPreviewDto>>(await posts
            .Skip((request.FilterBy.PageNumber - 1) * request.FilterBy.PageSize)
            .Take(request.FilterBy.PageSize)
            .ToListAsync(cancellationToken));


        return ApiResponse<GetPostsQuery.Result>.Success(new GetPostsQuery.Result
        {
            Items = items,
            TotalCount = totalCount,
            TotalPages = totalPages
        });
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
            yield return p => p.City.ToLower().Contains(cityLower);
        }
        
        if (filter.MinPrice.HasValue)
            yield return p => p.Price >= filter.MinPrice.Value;
        if (filter.MaxPrice.HasValue)
            yield return p => p.Price <= filter.MaxPrice;
    }
}