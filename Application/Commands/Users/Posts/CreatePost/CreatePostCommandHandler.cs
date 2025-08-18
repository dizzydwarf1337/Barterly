using Application.Core.ApiResponse;
using Application.Core.Factories.Interfaces;
using Application.Events.Posts.PostCreatedEvent;
using Application.Interfaces;
using Domain.Entities.Posts;
using Domain.Enums.Common;
using Domain.Interfaces.Commands.Post;
using MediatR;

namespace Application.Commands.Users.Posts.CreatePost;

public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, ApiResponse<Unit>>
{
    private readonly IFileService _fileService;
    private readonly ILogService _logService;
    private readonly IMediator _mediator;
    private readonly IPostCommandRepository _postCommandRepository;
    private readonly IPostFactory _postFactory;

    public CreatePostCommandHandler(IMediator mediator, ILogService logService, IPostFactory postFactory,
        IFileService fileService, IPostCommandRepository postCommandRepository)
    {
        _mediator = mediator;
        _logService = logService;
        _postFactory = postFactory;
        _fileService = fileService;
        _postCommandRepository = postCommandRepository;
    }

    public async Task<ApiResponse<Unit>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var postToAdd = _postFactory.CreatePost(
            request.SubCategoryId,
            request.AuthorizeData.UserId,
            request.PostType,
            request.Title,
            request.FullDescription,
            request.ShortDescription,
            cancellationToken,
            request.Currency,
            request.City,
            request.Region,
            request.Country,
            request.Street,
            request.Price,
            request.PostPriceType,
            request.Tags,
            request.MainImage,
            request.SecondaryImages,
            request.RentObjectType,
            request.NumberOfRooms,
            request.Area,
            request.Floor,
            request.Workload,
            request.WorkLocation,
            request.MinSalary,
            request.MaxSalary,
            request.BuildingNumber,
            request.ExperienceRequired
        );

        if (request.MainImage != null)
        {
            var mainImagePath = await _fileService.SaveFile(request.MainImage);
            postToAdd.MainImageUrl = mainImagePath;
        }

        if (request.SecondaryImages != null && request.SecondaryImages.Length != 0)
            foreach (var image in request.SecondaryImages)
            {
                var imagePath = await _fileService.SaveFile(image);
                var postImage = new PostImage
                {
                    PostId = postToAdd.Id,
                    ImageUrl = imagePath
                };
                postToAdd.PostImages ??= [];
                postToAdd.PostImages.Add(postImage);
            }

        var post = await _postCommandRepository.CreatePostAsync(postToAdd, cancellationToken);

        await _mediator.Publish(new PostCreatedEvent
        {
            UserId = request.AuthorizeData.UserId,
            PostId = post.Id
        });
        await _logService.CreateLogAsync($"Post created title: {post.Title}", cancellationToken,
            LogType.Information, post.Id.ToString(), post.OwnerId);
        return ApiResponse<Unit>.Success(Unit.Value, 201);
    }
}