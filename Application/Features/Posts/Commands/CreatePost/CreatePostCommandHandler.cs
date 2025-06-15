using API.Core.ApiResponse;
using Application.Core.Factories.Interfaces;
using Application.DTOs.Posts;
using Application.Features.Posts.Events.PostCreatedEvent;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.Posts;
using Domain.Enums.Common;
using Domain.Interfaces.Commands.Post;
using MediatR;

namespace Application.Features.Posts.Commands.CreatePost
{
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, ApiResponse<PostDto>>
    {
        private readonly IMediator _mediator;
        private readonly ILogService _logService;
        private readonly IPostFactory _postFactory;
        private readonly IFileService _fileService;
        private readonly IPostCommandRepository _postCommandRepository;
        private readonly IMapper _mapper;
        public CreatePostCommandHandler( IMediator mediator,ILogService logService, IPostFactory postFactory, IFileService fileService, IPostCommandRepository postCommandRepository, IMapper mapper)
        {
            _mediator = mediator;
            _logService = logService;
            _postFactory = postFactory;
            _fileService = fileService;
            _postCommandRepository = postCommandRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<PostDto>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {

            var postToAdd = _postFactory.CreatePost(request.Post);

            if (request.Post.MainImage != null)
            {
                var mainImagePath = await _fileService.SaveFile(request.Post.MainImage);
                postToAdd.MainImageUrl = mainImagePath;
            }
            if (request.Post.SecondaryImages != null && request.Post.SecondaryImages.Length != 0)
            {
                foreach (var image in request.Post.SecondaryImages)
                {
                    var imagePath = await _fileService.SaveFile(image);
                    var postImage = new PostImage()
                    {
                        PostId = postToAdd.Id,
                        ImageUrl = imagePath,
                    };
                    postToAdd.PostImages ??= [];
                    postToAdd.PostImages.Add(postImage);
                }
            }
            var post = await _postCommandRepository.CreatePostAsync(postToAdd);
            
            await _mediator.Publish(new PostCreatedEvent
            {
                UserId = Guid.Parse(request.Post.OwnerId),
                PostId = post.Id
            });
            await _logService.CreateLogAsync($"Post created title: {post.Title}", LogType.Information,post.Id.ToString(),post.OwnerId);
            return ApiResponse<PostDto>.Success(_mapper.Map<PostDto>(post), 201);

        }
    }
}
