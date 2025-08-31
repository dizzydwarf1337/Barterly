using Application.Core.ApiResponse;
using Application.Events.Posts.PostRejectedEvent;
using Application.Interfaces;
using Domain.Enums.Common;
using Domain.Enums.Posts;
using Domain.Interfaces.Commands.Post;
using MediatR;

namespace Application.Commands.Moderators.Posts.RejectPost;

public class RejectPostCommandHandler : IRequestHandler<RejectPostCommand, ApiResponse<Unit>>
{
    private readonly ILogService _logService;
    private readonly IMediator _mediator;
    private readonly IPostSettingsCommandRepository _postSettingsCommandRepository;

    public RejectPostCommandHandler(IPostSettingsCommandRepository postSettingsCommandReposiotry,
        IMediator mediator, ILogService logService)
    {
        _postSettingsCommandRepository = postSettingsCommandReposiotry;
        _mediator = mediator;
        _logService = logService;
    }

    public async Task<ApiResponse<Unit>> Handle(RejectPostCommand request, CancellationToken cancellationToken)
    {
        await _postSettingsCommandRepository.UpdatePostSettings(request.PostId, cancellationToken, true, false,
            PostStatusType.Rejected, request.Reason);
        await _mediator.Publish(new PostRejectedEvent { postId = request.PostId, reason = request.Reason });
        await _logService.CreateLogAsync($"Post rejected: {request.PostId}", cancellationToken,
            LogType.Information, postId: request.PostId, userId: request.AuthorizeData.UserId);
        return ApiResponse<Unit>.Success(Unit.Value);
    }
}