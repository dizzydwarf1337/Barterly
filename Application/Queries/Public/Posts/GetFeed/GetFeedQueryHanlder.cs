using Application.Core.ApiResponse;
using Application.DTOs.Posts;
using AutoMapper;
using Domain.Entities.Posts;
using Domain.Enums.Posts;
using Domain.Interfaces.Queries.Post;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Public.Posts.GetFeed;

public class GetFeedQueryHanlder : IRequestHandler<GetFeedQuery, ApiResponse<ICollection<PostPreviewDto>>>
{
    private readonly IMapper _mapper;
    private readonly IPostQueryRepository _postRepository;

    public GetFeedQueryHanlder(IPostQueryRepository postQueryRepository, IMapper mapper)
    {
        _postRepository = postQueryRepository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<ICollection<PostPreviewDto>>> Handle(GetFeedQuery request,
        CancellationToken cancellationToken)
    {
        return ApiResponse<ICollection<PostPreviewDto>>.Success(
            ShufflePosts(
                await GetPosts(request.PageSize, request.PageNumber, cancellationToken),
                await GetPromotedPosts((int)(request.PageSize * (1.0 / 3)), request.PageNumber,
                    cancellationToken)));
    }

    private async Task<ICollection<Post>> GetPosts(int PageSize, int pageNumber, CancellationToken token)
    {
        var postsQuery = _postRepository.GetAllPosts()
            .Where(x =>
                x.PostSettings.postStatusType == PostStatusType.Published &&
                !x.PostSettings.IsDeleted
                && x.Promotion.Type == PostPromotionType.None)
            .OrderByDescending(x => x.CreatedAt)
            .Skip((pageNumber - 1) * PageSize)
            .Take(PageSize);
        var posts = await postsQuery.ToListAsync(token);
        return posts;
    }

    private async Task<ICollection<Post>> GetPromotedPosts(int count, int pageNumber, CancellationToken token)
    {
        var topPostsCount = (int)Math.Ceiling(count * 2 / 3.0);
        var highlightPostsCount = count - topPostsCount;

        var topPostsQuery = _postRepository.GetAllPosts()
            .Where(x =>
                x.Promotion.Type == PostPromotionType.Top &&
                x.PostSettings.postStatusType == PostStatusType.Published &&
                !x.PostSettings.IsDeleted)
            .OrderByDescending(_ => Guid.NewGuid())
            .Skip((pageNumber - 1) * topPostsCount)
            .Take(topPostsCount)
            .ToListAsync(token);

        var highlightPostsQuery = _postRepository.GetAllPosts()
            .Where(x =>
                x.Promotion.Type == PostPromotionType.Highlight &&
                x.PostSettings.postStatusType == PostStatusType.Published &&
                !x.PostSettings.IsDeleted)
            .OrderByDescending(x => x.ViewsCount)
            .Skip((pageNumber - 1) * highlightPostsCount)
            .Take(highlightPostsCount)
            .ToListAsync(token);

        var topPosts = await topPostsQuery;
        var highlightPosts = await highlightPostsQuery;

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

        return _mapper.Map<List<PostPreviewDto>>(result);
    }
}