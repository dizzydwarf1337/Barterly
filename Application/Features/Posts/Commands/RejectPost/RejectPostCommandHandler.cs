using API.Core.ApiResponse;
using Application.Features.Posts.Events.PostRejectedEvent;
using Application.Interfaces;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Commands.Post;
using MediatR;
using Persistence.Repositories.Queries.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Posts.Commands.RejectPost
{
    public class RejectPostCommandHandler : IRequestHandler<RejectPostCommand, ApiResponse<Unit>>
    {
        private readonly IPostSettingsCommandRepository _postSettingsCommandRepository;
        private readonly IMediator _mediator;
        private readonly ILogService _logService;

        public RejectPostCommandHandler(IPostSettingsCommandRepository postSettingsCommandRepository, IMediator mediator, ILogService logService)
        {
            _postSettingsCommandRepository = postSettingsCommandRepository;
            _mediator = mediator;
            _logService = logService;
        }

        public async Task<ApiResponse<Unit>> Handle(RejectPostCommand request, CancellationToken cancellationToken)
        {

            await _postSettingsCommandRepository.UpdatePostSettings(Guid.Parse(request.RejectPostDto.postId), true, false, Domain.Enums.Posts.PostStatusType.Rejected, request.RejectPostDto.reason);
            await _mediator.Publish(new PostRejectedEvent { postId = request.RejectPostDto.postId, reason = request.RejectPostDto.reason });
            await _logService.CreateLogAsync($"Post rejected: {request.RejectPostDto.postId}", Domain.Enums.Common.LogType.Information, postId: Guid.Parse(request.RejectPostDto.postId));
            return ApiResponse<Unit>.Success(Unit.Value, 200);

        }
    }
}
