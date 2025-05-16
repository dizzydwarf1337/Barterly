using API.Core.ApiResponse;
using Application.Features.Posts.Events.PostApprovedEvent;
using Application.Interfaces;
using Domain.Exceptions.BusinessExceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Posts.Commands.ApprovePost
{
    public class ApprovePostCommandHandler : IRequestHandler<ApprovePostCommand, ApiResponse<Unit>>
    {
        private readonly IPostSettingsService _postSettingsService;
        private readonly IMediator _mediator;
        private readonly ILogService _logService;
        public ApprovePostCommandHandler(IPostSettingsService postSettingsService, IMediator mediator, ILogService logService)
        {
            _postSettingsService = postSettingsService;
            _mediator = mediator;
            _logService = logService;
        }
        public async Task<ApiResponse<Unit>> Handle(ApprovePostCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _postSettingsService.ApprovePost(request.ApprovePostDto);
                await _mediator.Publish(new PostApprovedEvent { postId = request.ApprovePostDto.postId });
                await _logService.CreateLogAsync($"Post approved: {request.ApprovePostDto.postId}", Domain.Enums.Common.LogType.Information, postId: Guid.Parse(request.ApprovePostDto.postId));
                return ApiResponse<Unit>.Success(Unit.Value, 200);
            }
            catch (EntityNotFoundException)
            {
                return ApiResponse<Unit>.Failure("Post not found", 404);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message + ex.StackTrace);
                return ApiResponse<Unit>.Failure("Error while approving post");
            }
        }
    }
}
