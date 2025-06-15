using API.Core.ApiResponse;
using Application.Features.Posts.Events.PostDeletedEvent;
using Application.Interfaces;
using AutoMapper;
using Domain.Enums.Common;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Commands.Post;
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
        private readonly IPostSettingsCommandRepository _postSettingsCommandRepository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ILogService _logService;

        public DeletePostCommandHandler(IPostSettingsCommandRepository postSettingsCommandRepository, IMapper mapper, IMediator mediator, ILogService logService)
        {
            _postSettingsCommandRepository = postSettingsCommandRepository;
            _mapper = mapper;
            _mediator = mediator;
            _logService = logService;
        }

        public async Task<ApiResponse<Unit>> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            var postID = Guid.Parse(request.PostId);
            await _postSettingsCommandRepository.UpdatePostSettings(postID,true,true,Domain.Enums.Posts.PostStatusType.Deleted,null);
            await _mediator.Publish(new PostDeletedEvent { postId = postID });
            await _logService.CreateLogAsync($"Post deleted id: {request.PostId}", LogType.Information, postId: postID);
            return ApiResponse<Unit>.Success(Unit.Value);
        }
    }
}
