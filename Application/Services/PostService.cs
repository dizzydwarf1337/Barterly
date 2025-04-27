using Application.Core.Factories.Interfaces;
using Application.Core.Factories.PostFactory;
using Application.DTOs.Posts;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.Posts;
using Domain.Enums;
using Domain.Interfaces.Commands.Post;
using Domain.Interfaces.Queries.Post;
using System.Diagnostics;

namespace Application.Services
{
    public class PostService : IPostService
    {
        private readonly IPostCommandRepository _postCommandRepository;
        private readonly IPostQueryRepository _postQueryRepository;
        private readonly IPostImageCommandRepository _postImageCommandRepository;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;
        private readonly IPostFactory _postFactory;
        public PostService(IPostCommandRepository postCommandRepository,
            IPostQueryRepository postQueryRepository,
            IMapper mapper,
            IFileService fileService,
            IPostImageCommandRepository postImageCommandRepository,
            IPostFactory postFactory
            )
        {
            _postCommandRepository = postCommandRepository;
            _postQueryRepository = postQueryRepository;
            _mapper = mapper;
            _fileService = fileService;
            _postImageCommandRepository = postImageCommandRepository;
            _postFactory = postFactory;
        }


        public async Task<PostDto> CreatePost(CreatePostDto post)
        {
            var postToAdd = _postFactory.CreatePost(post);
            //Images
            if (post.MainImage != null)
            {
                Console.WriteLine("Main image creating...");
                var mainImagePath = await _fileService.SaveFile(post.MainImage);
                postToAdd.MainImageUrl = mainImagePath;
            }
            if (post.SecondaryImages != null)
            {
                foreach (var image in post.SecondaryImages)
                {
                    var imagePath = await _fileService.SaveFile(image);
                    var postImage = new PostImage()
                    {
                        PostId = postToAdd.Id,
                        ImageUrl = imagePath,
                    };
                    postToAdd.PostImages.Add(postImage);
                }
            }

            return _mapper.Map<PostDto>(await _postCommandRepository.CreatePostAsync(postToAdd));
        }

        public async Task<ICollection<PostPreviewDto>> GetUserFavouritePostsPaginated(Guid userId, int PageSize, int PageNumber)
        {
            var posts = await _postQueryRepository.GetUserFavouritePostsPaginated(userId, PageSize,PageNumber);
            return _mapper.Map<ICollection<PostPreviewDto>>(posts);
        }


        public async Task<PostDto> GetPostById(Guid postId)
        {
            var post = await _postQueryRepository.GetPostById(postId);
            return _mapper.Map<PostDto>(post);
        }


        public async Task<ICollection<PostPreviewDto>> GetPostsByUserIdPaginated(Guid userId, int PageSize, int PageNumber, Guid? currentUserId = null)
        {
            var posts = await _postQueryRepository.GetPostsByOwnerIdPaginated(userId,PageSize,PageNumber,currentUserId);
            return _mapper.Map<ICollection<PostPreviewDto>>(posts);
        }

        public async Task UpdatePost(PostDto post)
        {
            var postUp = _mapper.Map<Post>(post);
            await _postCommandRepository.UpdatePostAsync(postUp);
        }


    }
}
