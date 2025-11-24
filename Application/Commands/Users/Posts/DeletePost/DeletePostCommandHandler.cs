using Application.Core.ApiResponse;
using Application.Events.Posts.PostDeletedEvent;
using Application.Interfaces;
using AutoMapper;
using Domain.Enums.Common;
using Domain.Enums.Posts;
using Domain.Interfaces.Commands.Post;
using Domain.Interfaces.Queries.Post;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.Users.Posts.DeletePost;

public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, ApiResponse<Unit>>
{
    private readonly ILogService _logService;
    private readonly IPostQueryRepository _postQueryRepository;
    private readonly IMediator _mediator;
    private readonly IPostSettingsCommandRepository _postSettingsCommandRepository;

    public DeletePostCommandHandler(IPostSettingsCommandRepository postSettingsCommandRepository, IPostQueryRepository postQueryRepository,
        IMediator mediator, ILogService logService)
    {
        _postSettingsCommandRepository = postSettingsCommandRepository;
        _postQueryRepository = postQueryRepository;
        _mediator = mediator;
        _logService = logService;
    }

    public async Task<ApiResponse<Unit>> Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var post = await _postQueryRepository.GetAllPosts()
            .Where(x => x.Id == request.PostId && x.OwnerId == request.AuthorizeData!.UserId).FirstOrDefaultAsync(cancellationToken);
        if(post == null)
            return ApiResponse<Unit>.Failure("Post not found", 404);
        await _postSettingsCommandRepository.UpdatePostSettings(post.PostSettingsId, cancellationToken, true, true,
            PostStatusType.Deleted, null);
        await _mediator.Publish(new PostDeletedEvent { postId = request.PostId }, cancellationToken);
        await _logService.CreateLogAsync($"Post deleted id: {request.PostId}", cancellationToken,
            LogType.Information, postId: request.PostId);
        return ApiResponse<Unit>.Success(Unit.Value);
    }
}