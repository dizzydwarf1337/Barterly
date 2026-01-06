using Application.Core.ApiResponse;
using Application.DTOs.Posts;
using AutoMapper;
using Domain.Interfaces.Commands.Post;
using Domain.Interfaces.Queries.Post;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Users.Posts.GetPostById;

public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, ApiResponse<PostDto>>
{
    private readonly IMapper _mapper;
    private readonly IPostQueryRepository _postQueryRepository;
    private readonly IPostCommandRepository _postCommandRepository;
    private readonly IVisitedPostQueryRepository _visitedPostQueryRepository;
    private readonly IVisitedPostCommandRepository  _visitedPostCommandRepository;

    public GetPostByIdQueryHandler(
        IPostQueryRepository postQueryRepository,
        IPostCommandRepository postCommandRepository,
        IVisitedPostQueryRepository visitedPostQueryRepository,
        IVisitedPostCommandRepository visitedPostCommandRepository,
        IMapper mapper)
    {
        _postQueryRepository = postQueryRepository;
        _postCommandRepository = postCommandRepository;
        _visitedPostQueryRepository = visitedPostQueryRepository;
        _visitedPostCommandRepository = visitedPostCommandRepository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<PostDto>> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        var post = await _postQueryRepository.GetAllPosts()
            .Where(post =>
                post.Id == request.PostId &&
                (
                    (post.OwnerId == request.AuthorizeData!.UserId && !post.PostSettings.IsDeleted)
                    ||
                    (!post.PostSettings.IsHidden && !post.PostSettings.IsDeleted)
                )
            )
            .Include(x => x.PostImages)
            .FirstOrDefaultAsync(cancellationToken);
        if (post == null)
            return ApiResponse<PostDto>.Failure("Post not found or is hidden/deleted.", 404);
        
        var visitedPost = await _visitedPostQueryRepository.GetUserVisitedPost(post.Id, request.AuthorizeData!.UserId, cancellationToken);
        if (visitedPost?.PostId != null)
        {
            visitedPost.LastVisitedAt = DateTimeOffset.Now.DateTime;
            visitedPost.VisitedCount += 1;
            await _visitedPostCommandRepository.UpdateVisitedPost(visitedPost, cancellationToken);
        }
        else
        {
            await _visitedPostCommandRepository.VisitPost(post.Id, request.AuthorizeData.UserId, cancellationToken);
            await _postCommandRepository.IncreasePostView(post.Id, cancellationToken);
        }
        
        return ApiResponse<PostDto>.Success(_mapper.Map<PostDto>(post));
    }
}