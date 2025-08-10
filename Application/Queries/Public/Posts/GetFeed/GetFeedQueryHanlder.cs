using Application.Core.ApiResponse;
using Application.DTOs.Posts;
using AutoMapper;
using Domain.Entities.Posts;
using Domain.Enums.Posts;
using Domain.Interfaces.Queries.Post;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Public.Posts.GetFeed;

public class GetFeedQueryHanlder : IRequestHandler<GetFeedQuery, ApiResponse<GetFeedQuery.Result>>
{
    private readonly IMapper _mapper;
    private readonly IPostQueryRepository _postRepository;

    public GetFeedQueryHanlder(IPostQueryRepository postQueryRepository, IMapper mapper)
    {
        _postRepository = postQueryRepository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<GetFeedQuery.Result>> Handle(GetFeedQuery request,
        CancellationToken cancellationToken)
    {
        var posts = await GetPosts(request.FilterBy.PageSize, request.FilterBy.PageNumber, request.SortBy, cancellationToken);
        var promotedPosts = await GetPromotedPosts((int)(request.FilterBy.PageSize / 3.0), request.FilterBy.PageNumber, cancellationToken);

        var shuffledPosts = ShufflePosts(posts, promotedPosts);
        
        var totalCount = await _postRepository.GetAllPosts()
            .Where(p =>
                p.PostSettings.postStatusType == PostStatusType.Published &&
                !p.PostSettings.IsDeleted &&
                p.Promotion.Type == PostPromotionType.None)
            .CountAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling(totalCount / (double)request.FilterBy.PageSize);

        var result = new GetFeedQuery.Result
        {
            Items = shuffledPosts,
            TotalCount = totalCount,
            TotalPages = totalPages
        };

        return ApiResponse<GetFeedQuery.Result>.Success(result);
    }

    private async Task<ICollection<Post>> GetPosts(int pageSize, int pageNumber, GetFeedQuery.SortSpecification? sortBy, CancellationToken token)
    {
        var query = _postRepository.GetAllPosts()
            .Where(x =>
                x.PostSettings.postStatusType == PostStatusType.Published &&
                !x.PostSettings.IsDeleted &&
                x.Promotion.Type == PostPromotionType.None);

        if (sortBy != null && !string.IsNullOrEmpty(sortBy.SortBy))
        {
            query = (sortBy.SortBy.ToLower()) switch
            {
                "createdat" => sortBy.IsDescending ? query.OrderByDescending(x => x.CreatedAt) : query.OrderBy(x => x.CreatedAt),
                "title" => sortBy.IsDescending ? query.OrderByDescending(x => x.Title) : query.OrderBy(x => x.Title),
                "viewscount" => sortBy.IsDescending ? query.OrderByDescending(x => x.ViewsCount) : query.OrderBy(x => x.ViewsCount),
                _ => query.OrderByDescending(x => x.CreatedAt) 
            };
        }
        else
        {
            query = query.OrderByDescending(x => x.CreatedAt);
        }

        query = query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        return await query.ToListAsync(token);
    }

    private async Task<ICollection<Post>> GetPromotedPosts(int count, int pageNumber, CancellationToken token)
    {
        var topPostsCount = (int)Math.Ceiling(count * 2 / 3.0);
        var highlightPostsCount = count - topPostsCount;

        var topPosts = await _postRepository.GetAllPosts()
            .Where(x =>
                x.Promotion.Type == PostPromotionType.Top &&
                x.PostSettings.postStatusType == PostStatusType.Published &&
                !x.PostSettings.IsDeleted)
            .OrderByDescending(_ => Guid.NewGuid())
            .Skip((pageNumber - 1) * topPostsCount)
            .Take(topPostsCount)
            .ToListAsync(token);

        var highlightPosts = await _postRepository.GetAllPosts()
            .Where(x =>
                x.Promotion.Type == PostPromotionType.Highlight &&
                x.PostSettings.postStatusType == PostStatusType.Published &&
                !x.PostSettings.IsDeleted)
            .OrderByDescending(x => x.ViewsCount)
            .Skip((pageNumber - 1) * highlightPostsCount)
            .Take(highlightPostsCount)
            .ToListAsync(token);
        

        return topPosts.Concat(highlightPosts).ToList();
    }

    private ICollection<PostPreviewDto> ShufflePosts(ICollection<Post> posts, ICollection<Post> promotedPosts)
    {
        var rnd = new Random();

        var shuffledRegular = posts.OrderBy(_ => rnd.Next()).ToList();
        var shuffledPromoted = promotedPosts.OrderBy(_ => rnd.Next()).ToList();

        var total = shuffledRegular.Count + shuffledPromoted.Count;
        var result = new List<Post>(total);

        var regularIndex = 0;
        var promotedIndex = 0;

        var regularRatio = shuffledRegular.Count / (double)total;
        var promotedRatio = shuffledPromoted.Count / (double)total;

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
                promotedCounter += 1 / promotedRatio;
            }
            else
            {
                result.Add(shuffledRegular[regularIndex++]);
                regularCounter += 1 / regularRatio;
            }
        }

        return _mapper.Map<ICollection<PostPreviewDto>>(result);
    }
}