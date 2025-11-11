using Application.Core.ApiResponse;
using Application.Events.Posts.PostDeletedEvent;
using Application.Interfaces;
using Domain.Enums.Common;
using Domain.Enums.Posts;
using Domain.Interfaces.Commands.Post;
using Domain.Interfaces.Queries.Post;
using MediatR;

namespace Application.Commands.Admins.Posts.DeletePost;

public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, ApiResponse<Unit>>
{
    private readonly ILogService _logService;
    private readonly IMediator _mediator;
    private readonly IPostSettingsCommandRepository _postSettingsCommandRepository;
    private readonly IPostSettingsQueryRepository _postSettingsQueryRepository;

    public DeletePostCommandHandler(IPostSettingsCommandRepository postSettingsCommandRepository, IPostSettingsQueryRepository postSettingsQueryRepository,
        IMediator mediator, ILogService logService)
    {
        _postSettingsCommandRepository = postSettingsCommandRepository;
        _postSettingsQueryRepository = postSettingsQueryRepository;
        _mediator = mediator;
        _logService = logService;
    }

    public async Task<ApiResponse<Unit>> Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var settings = await _postSettingsQueryRepository.GetPostSettingsByPostId(request.PostId, cancellationToken);
        await _postSettingsCommandRepository.UpdatePostSettings(settings.Id, cancellationToken, false, true,
            PostStatusType.Deleted, null);
        await _mediator.Publish(new PostDeletedEvent { postId = request.PostId });
        await _logService.CreateLogAsync($"Post deleted id: {request.PostId}", cancellationToken,
            LogType.Information, postId: request.PostId, userId: request.AuthorizeData.UserId);
        return ApiResponse<Unit>.Success(Unit.Value);
    }
}