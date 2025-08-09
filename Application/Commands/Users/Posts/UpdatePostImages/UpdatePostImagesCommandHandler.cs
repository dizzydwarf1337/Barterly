using Application.Core.ApiResponse;
using Application.Events.Posts.PostUpdatedEvent;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.Posts;
using Domain.Enums.Common;
using Domain.Enums.Posts;
using Domain.Interfaces.Commands.Post;
using MediatR;

namespace Application.Commands.Users.Posts.UpdatePostImages;

public class UpdatePostImagesCommandHandler : IRequestHandler<UpdatePostImagesCommand, ApiResponse<Unit>>
{
    private readonly IFileService _fileService;
    private readonly ILogService _logService;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IPostCommandRepository _postCommandRepository;
    private readonly IPostImageCommandRepository _postImageCommandRepository;
    private readonly IPostSettingsCommandRepository _postSettingsCommandRepository;

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

    public async Task<ApiResponse<Unit>> Handle(UpdatePostImagesCommand request,
        CancellationToken cancellationToken)
    {
        var postId = request.ImagesDto.PostId;
        if (request.ImagesDto.MainImage != null)
        {
            var mainImagePath = await _fileService.SaveFile(request.ImagesDto.MainImage);
            await _postCommandRepository.UpdatePostMainImage(postId, mainImagePath, cancellationToken);
        }

        if (request.ImagesDto.SecondaryImages != null)
        {
            await _postImageCommandRepository.DeletePostImagesByPostIdAsync(postId, cancellationToken);
            foreach (var image in request.ImagesDto.SecondaryImages.ToList())
            {
                var imagePath = await _fileService.SaveFile(image);
                var postImage = new PostImage
                {
                    PostId = postId,
                    ImageUrl = imagePath
                };
                await _postImageCommandRepository.CreatePostImageAsync(postImage, cancellationToken);
            }
        }

        await _postSettingsCommandRepository.UpdatePostSettings(postId, true, false, PostStatusType.ReSubmitted, "",
            cancellationToken);
        await _mediator.Publish(new PostUpdatedEvent { PostId = postId, UserId = request.OwnerId });
        await _logService.CreateLogAsync("Post updated", cancellationToken, LogType.Information, postId: postId,
            userId: request.OwnerId);
        return ApiResponse<Unit>.Success(Unit.Value);
    }
}