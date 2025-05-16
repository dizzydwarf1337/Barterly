using API.Core.ApiResponse;
using Application.Features.Posts.Events.PostDeletedEvent;
using Application.Interfaces;
using Domain.Enums.Common;
using Domain.Exceptions.BusinessExceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Posts.Commands.DeletePost
{
    public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, ApiResponse<Unit>>
    {
        private readonly IPostSettingsService _postSettingsService;
        private readonly IMediator _mediator;
        private readonly ILogService _logService;
        public DeletePostCommandHandler(IPostSettingsService postSettingsService, IMediator mediator, ILogService logService)
        {
            _postSettingsService = postSettingsService;
            _mediator = mediator;
            _logService = logService;
        }
        public async Task<ApiResponse<Unit>> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _postSettingsService.DeletePost(request.PostId);
                await _mediator.Publish(new PostDeletedEvent { postId = request.PostId });
                await _logService.CreateLogAsync($"Post deleted id: {request.PostId}", LogType.Information, postId: request.PostId);
                return ApiResponse<Unit>.Success(Unit.Value);

            }
            catch (EntityNotFoundException ex)
            {
                return ApiResponse<Unit>.Failure(ex.Message, 404);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message + ex.StackTrace);
                return ApiResponse<Unit>.Failure("Error while deleting post");
            }
        }
    }
}
