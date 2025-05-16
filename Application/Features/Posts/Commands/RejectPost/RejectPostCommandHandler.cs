using API.Core.ApiResponse;
using Application.Features.Posts.Events.PostRejectedEvent;
using Application.Interfaces;
using Domain.Exceptions.BusinessExceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Posts.Commands.RejectPost
{
    public class RejectPostCommandHandler : IRequestHandler<RejectPostCommand, ApiResponse<Unit>>
    {
        private readonly IPostSettingsService _postSettingsService;
        private readonly IMediator _mediator;
        private readonly ILogService _logService;

        public RejectPostCommandHandler(IPostSettingsService postSettingsService, IMediator mediator, ILogService logService)
        {
            _postSettingsService = postSettingsService;
            _mediator = mediator;
            _logService = logService;
        }

        public async Task<ApiResponse<Unit>> Handle(RejectPostCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _postSettingsService.RejectPost(request.RejectPostDto);
                await _mediator.Publish(new PostRejectedEvent { postId = request.RejectPostDto.postId, reason = request.RejectPostDto.reason });
                await _logService.CreateLogAsync($"Post rejected: {request.RejectPostDto.postId}", Domain.Enums.Common.LogType.Information, postId: Guid.Parse(request.RejectPostDto.postId));
                return ApiResponse<Unit>.Success(Unit.Value, 200);

            }
            catch (EntityNotFoundException)
            {
                return ApiResponse<Unit>.Failure("Post not found", 404);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                return ApiResponse<Unit>.Failure("Error while rejecting post");
            }
        }
    }
}
