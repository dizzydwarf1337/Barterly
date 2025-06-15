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
using Domain.Interfaces.Queries.Post;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Posts.Commands.UpdatePostImages
{
    public class UpdatePostImagesCommandHandler : IRequestHandler<UpdatePostImagesCommand, ApiResponse<Unit>>
    {
        private readonly IPostImageCommandRepository _postImageCommandRepository;
        private readonly IPostSettingsCommandRepository _postSettingsCommandRepository;
        private readonly IPostCommandRepository _postCommandRepository;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly ILogService _logService;

        public UpdatePostImagesCommandHandler(IPostImageCommandRepository postImageCommandRepository,
            IPostSettingsCommandRepository postSettingsCommandRepository,
            IMediator mediator,
            IMapper mapper,
            IFileService fileService, 
            ILogService logService, 
            IPostCommandRepository postCommandRepository)
        {
            _postImageCommandRepository = postImageCommandRepository;
            _postSettingsCommandRepository = postSettingsCommandRepository;
            _mediator = mediator;
            _mapper = mapper;
            _fileService = fileService;
            _logService = logService;
            _postCommandRepository = postCommandRepository;
        }

        public async Task<ApiResponse<Unit>> Handle(UpdatePostImagesCommand request, CancellationToken cancellationToken)
        {
            var postId = Guid.Parse(request.ImagesDto.PostId);
            if (request.ImagesDto.MainImage != null)
            {
                var mainImagePath = await _fileService.SaveFile(request.ImagesDto.MainImage);
                await _postCommandRepository.UpdatePostMainImage(postId, mainImagePath);
            }
            if (request.ImagesDto.SecondaryImages != null)
            {

                await _postImageCommandRepository.DeletePostImagesByPostIdAsync(postId);
            foreach (var image in request.ImagesDto.SecondaryImages.ToList())
            {

                var imagePath = await _fileService.SaveFile(image);
                var postImage = new PostImage()
                {
                    PostId = postId,
                    ImageUrl = imagePath,
                };
                await _postImageCommandRepository.CreatePostImageAsync(postImage);
            }
            }
            await _postSettingsCommandRepository.UpdatePostSettings(postId,true,false,PostStatusType.ReSubmitted,"");
            await _mediator.Publish(new PostUpdatedEvent { PostId = postId, UserId=Guid.Parse(request.ImagesDto.UserId)});
            await _logService.CreateLogAsync($"Post updated", LogType.Information, postId: postId, userId: Guid.Parse(request.ImagesDto.UserId));
            return ApiResponse<Unit>.Success(Unit.Value, 200);

        }
    }
}
