using Application.Core.ApiResponse;
using Application.Events.Posts.PostApprovedEvent;
using Application.Interfaces;
using Domain.Enums.Common;
using Domain.Enums.Posts;
using Domain.Interfaces.Commands.Post;
using MediatR;

namespace Application.Commands.Admins.Posts.ApprovePost;

public class ApprovePostCommandHandler : IRequestHandler<ApprovePostCommand, ApiResponse<Unit>>
{
    private readonly ILogService _logService;
    private readonly IMediator _mediator;
    private readonly IPostSettingsCommandRepository _postSettingsCommandRepository;

    public ApprovePostCommandHandler(IMediator mediator, ILogService logService,
        IPostSettingsCommandRepository postSettingsCommandRepository)
    {
        _mediator = mediator;
        _logService = logService;
        _postSettingsCommandRepository = postSettingsCommandRepository;
    }

    public async Task<ApiResponse<Unit>> Handle(ApprovePostCommand request, CancellationToken cancellationToken)
    {
        await UpdatePostSettings(request.PostId, cancellationToken);
        await _mediator.Publish(new PostApprovedEvent { postId = request.PostId });
        await _logService.CreateLogAsync($"Post approved: {request.PostId}", cancellationToken,
            LogType.Information, postId: request.PostId, userId: request.AuthorizeData.UserId);
        return ApiResponse<Unit>.Success(Unit.Value);
    }

    private async Task UpdatePostSettings(Guid postId, CancellationToken token)
    {
        await _postSettingsCommandRepository.UpdatePostSettings(postId, false, false,
            PostStatusType.Published, null, token);
    }
}