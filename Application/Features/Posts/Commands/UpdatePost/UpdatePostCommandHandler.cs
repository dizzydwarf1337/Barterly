using API.Core.ApiResponse;
using Application.DTOs.Posts;
using Application.Features.Posts.Events.PostUpdatedEvent;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Domain.Entities.Posts;
using Domain.Enums.Common;
using Domain.Enums.Posts;
using Domain.Exceptions.BusinessExceptions;
using Domain.Exceptions.DataExceptions;
using Domain.Interfaces.Commands.Post;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Posts.Commands.UpdatePost
{
    public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, ApiResponse<PostDto>>
    {
        private readonly IPostCommandRepository _postCommandRepository;
        private readonly IPostSettingsCommandRepository _postSettingsCommandRepository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ILogService _logService;

        public UpdatePostCommandHandler(IPostCommandRepository postCommandRepository,IPostSettingsCommandRepository postSettingsCommandRepository ,IMapper mapper, IMediator mediator, ILogService logService)
        {
            
            _postCommandRepository = postCommandRepository;
            _postSettingsCommandRepository = postSettingsCommandRepository;
            _mapper = mapper;
            _mediator = mediator;
            _logService = logService;
        }

        public async Task<ApiResponse<PostDto>> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            var post = _mapper.Map<Post>(request.post);
            await _postCommandRepository.UpdatePostAsync(post);
            await _postSettingsCommandRepository.UpdatePostSettings(post.Id, true, false, PostStatusType.ReSubmitted, "");
            await _mediator.Publish(new PostUpdatedEvent { PostId = post.Id, UserId = post.OwnerId});
            await _logService.CreateLogAsync($"Post updated title: {post.Title}", LogType.Information, postId: post.Id, userId: post.OwnerId);
            return ApiResponse<PostDto>.Success(_mapper.Map<PostDto>(post), 200);

         
        }
    }
}
