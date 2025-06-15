using API.Core.ApiResponse;
using Application.Features.Posts.Events.PostApprovedEvent;
using Application.Interfaces;
using Domain.Interfaces.Commands.Post;
using Domain.Interfaces.Queries.Post;
using MediatR;

namespace Application.Features.Posts.Commands.ApprovePost
{
    public class ApprovePostCommandHandler : IRequestHandler<ApprovePostCommand, ApiResponse<Unit>>
    {
        private readonly IMediator _mediator;
        private readonly ILogService _logService;
        private readonly IPostSettingsQueryRepository _postSettingsQueryRepository;
        private readonly IPostSettingsCommandRepository _postSettingsCommandRepository;

        public ApprovePostCommandHandler(IMediator mediator, ILogService logService, IPostSettingsQueryRepository postQueryRepository, IPostSettingsCommandRepository postSettingsCommandRepository)
        {
            _mediator = mediator;
            _logService = logService;
            _postSettingsQueryRepository = postQueryRepository;
            _postSettingsCommandRepository = postSettingsCommandRepository;
        }
        public async Task<ApiResponse<Unit>> Handle(ApprovePostCommand request, CancellationToken cancellationToken)
        {
          
            var postSettings = await _postSettingsQueryRepository.GetPostSettingsByPostId(Guid.Parse(request.ApprovePostDto.postId));
            postSettings.IsHidden = false;
            postSettings.postStatusType = Domain.Enums.Posts.PostStatusType.Published;
            postSettings.RejectionMessage = null;
            await _postSettingsCommandRepository.UpdatePostSettings(postSettings.PostId,postSettings.IsHidden,postSettings.IsDeleted,postSettings.postStatusType,postSettings.RejectionMessage);
            await _mediator.Publish(new PostApprovedEvent { postId = request.ApprovePostDto.postId });
            await _logService.CreateLogAsync($"Post approved: {request.ApprovePostDto.postId}", Domain.Enums.Common.LogType.Information, postId: Guid.Parse(request.ApprovePostDto.postId));
            return ApiResponse<Unit>.Success(Unit.Value, 200);

        }
    }
}
