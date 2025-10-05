using System.Linq.Expressions;
using Application.Core.ApiResponse;
using Application.DTOs.Posts;
using AutoMapper;
using Domain.Entities.Posts;
using Domain.Interfaces.Queries.Post;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Admins.Posts.GetPostFiltredPaginated;

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
        
        if (request.FilterBy == null)
            return ApiResponse<GetPostsQuery.Result>.Failure("Invalid filter parameters.");
        
        if (request.FilterBy?.PageSize <= 0 || request.FilterBy?.PageNumber <= 0)
            return ApiResponse<GetPostsQuery.Result>.Failure("Invalid pagination parameters.");


        
        var posts = _postQueryRepository.GetAllPosts();

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
                Posts = items,
                TotalCount = totalCount,
                TotalPages = totalPages
            });
    }

    private IEnumerable<Expression<Func<Post, bool>>> GetFilters(GetPostsQuery.FilterSpecification filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Search))
            yield return p => p.Title.ToLower().Contains(filter.Search.ToLower());

        if (filter.CategoryId.HasValue && filter.CategoryId.Value != Guid.Empty)
            yield return p => p.SubCategory.CategoryId == filter.CategoryId;

        if (filter.SubCategoryId.HasValue && filter.SubCategoryId != Guid.Empty)
            yield return p => p.SubCategoryId == filter.SubCategoryId;

        if (filter.UserId.HasValue && filter.UserId != Guid.Empty)
            yield return p => p.OwnerId == filter.UserId;

        if (filter.IsActive.HasValue && filter.IsActive.Value)
            yield return p => !p.PostSettings.IsHidden && !p.PostSettings.IsDeleted;

        if (filter.IsDeleted.HasValue && filter.IsDeleted.Value)
            yield return p => p.PostSettings.IsDeleted;
    }
}