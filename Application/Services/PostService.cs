using Application.Core.Factories.Interfaces;
using Application.Core.Factories.PostFactory;
using Application.DTOs.Posts;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.Posts;
using Domain.Enums;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Commands.Post;
using Domain.Interfaces.Queries.Post;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.Runtime.CompilerServices;

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
        private readonly IPostSettingsService _postSettingsService;
        public PostService(IPostCommandRepository postCommandRepository,
            IPostQueryRepository postQueryRepository,
            IMapper mapper,
            IFileService fileService,
            IPostImageCommandRepository postImageCommandRepository,
            IPostFactory postFactory,
            IPostSettingsService postSettingsService
            )
        {
            _postCommandRepository = postCommandRepository;
            _postQueryRepository = postQueryRepository;
            _mapper = mapper;
            _fileService = fileService;
            _postImageCommandRepository = postImageCommandRepository;
            _postFactory = postFactory;
            _postSettingsService = postSettingsService;
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


        public async Task<PostDto> GetPostById(Guid postId, Guid? userId = null)
        {
            if (userId == null)
            {
               var  post = await _postQueryRepository.GetPostById(postId);
               return _mapper.Map<PostDto>(post);

            }
            else if(userId != null)
            {
               var post = await _postQueryRepository.GetPostByIdOwner(postId, userId.Value);
                return _mapper.Map<PostDto>(post);

            }
            throw new EntityNotFoundException("Post");
        }


        public async Task<ICollection<PostPreviewDto>> GetPostsByUserIdPaginated(Guid userId, int PageSize, int PageNumber, Guid? currentUserId = null)
        {
            var posts = await _postQueryRepository.GetPostsByOwnerIdPaginated(userId,PageSize,PageNumber,currentUserId);
            return _mapper.Map<ICollection<PostPreviewDto>>(posts);
        }

        public async Task<PostDto> UpdatePost(EditPostDto postDto)
        {
            var postFromDb = await _postQueryRepository.GetPostByIdOwner(Guid.Parse(postDto.Id), Guid.Parse(postDto.OwnerId));
            _mapper.Map(postDto, postFromDb);
            await _postSettingsService.ResubmitPost(postFromDb.Id);
            return _mapper.Map<PostDto>(await _postCommandRepository.UpdatePostAsync(postFromDb));
        }
        public async Task<PostDto> UploadImages(ImagesDto imagesDto)
        {
            var post = await _postQueryRepository.GetPostByIdOwner(Guid.Parse(imagesDto.PostId), Guid.Parse(imagesDto.UserId));
            if (imagesDto.MainImage != null)
            {
                var mainImagePath = await _fileService.SaveFile(imagesDto.MainImage);
                await _fileService.DeleteFile(post.MainImageUrl);
                post.MainImageUrl = mainImagePath;
            }
            if (imagesDto.SecondaryImages != null)
            {
                if (post.PostImages != null)
                {
                    foreach (var image in post.PostImages.ToList())
                    {
                        await _fileService.DeleteFile(image.ImageUrl);
                        await _postImageCommandRepository.DeletePostImageAsync(image.Id);
                    }
                }
                if(post.PostImages == null) post.PostImages = new List<PostImage>();
                foreach (var image in imagesDto.SecondaryImages.ToList())
                {

                    var imagePath = await _fileService.SaveFile(image);
                    var postImage = new PostImage()
                    {
                        PostId = post.Id,
                        ImageUrl = imagePath,
                    };
                    
                    post.PostImages.Add(postImage);
                    await _postImageCommandRepository.CreatePostImageAsync(postImage);
                }
            }
            await _postSettingsService.ResubmitPost(post.Id);
            return (_mapper.Map<PostDto>(await _postCommandRepository.UpdatePostAsync(post)));
        }
        public async Task<PostDto> GetPostByIdAdmin(Guid postId)
        {
            return _mapper.Map<PostDto>(await _postQueryRepository.GetPostByIdAdmin(postId));
        }
    }
}
