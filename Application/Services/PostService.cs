using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.Posts;
using Domain.Interfaces.Commands.Post;
using Domain.Interfaces.Queries.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PostService : IPostService
    {
        private readonly IPostCommandRepository _postCommandRepository;
        private readonly IPostQueryRepository _postQueryRepository;
        private readonly IMapper _mapper;
        public PostService(IPostCommandRepository postCommandRepository, IPostQueryRepository postQueryRepository, IMapper mapper)
        {
            _postCommandRepository = postCommandRepository;
            _postQueryRepository = postQueryRepository;
            _mapper = mapper;
        }

        public async Task ChangePostHiddenStatus(Guid postId, bool value)
        {
            await _postCommandRepository.SetHidePostAsync(postId, value);
        }

        public async Task CreatePost(PostDto post)
        {
            var postToAdd = _mapper.Map<Post>(post);
            await _postCommandRepository.CreatePostAsync(postToAdd);
        }

        public async Task DeletePost(Guid postId)
        {
            await _postCommandRepository.DeletePostAsync(postId);
        }

        public async Task<ICollection<PostDto>> GetAllPostsAsync()
        {
            var posts = await _postQueryRepository.GetAllPostsAsync();
            return _mapper.Map<ICollection<PostDto>>(posts);
        }


        public async Task<ICollection<PostDto>> GetUserFavouritePosts(Guid userId)
        {
            var posts = await _postQueryRepository.GetUserFavouritePosts(userId);
            return _mapper.Map<ICollection<PostDto>>(posts);
        }

        public async Task<ICollection<PostDto>> GetPaginatedPosts(int PageSize, int PageNumber)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<PostDto>> GetPaginatedPostsByCategoryId(Guid categoryId, int PageSize, int PageNumber)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<PostDto>> GetPaginatedPostsBySubCategoryId(Guid subCategoryId, int PageSize, int PageNumber)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<PostDto>> GetPaginatedPostsByUserId(Guid userId, int PageSize, int PageNumber)
        {
            throw new NotImplementedException();
        }

        public async Task<PostDto> GetPostById(Guid postId)
        {
            var post = await _postQueryRepository.GetPostByIdAsync(postId);
            return _mapper.Map<PostDto>(post);
        }

        public async Task<ICollection<PostDto>> GetPostsBySubCategoryId(Guid subCategoryId)
        {
            var posts = await _postQueryRepository.GetFiltredPostsAsync(1,10,subCategoryId:subCategoryId);
            return _mapper.Map<ICollection<PostDto>>(posts);
        }

        public async Task<ICollection<PostDto>> GetPostsByUserId(Guid userId)
        {
            var posts = await _postQueryRepository.GetPostsByOwnerIdAsync(userId);
            return _mapper.Map<ICollection<PostDto>>(posts);
        }

        public async Task UpdatePost(PostDto post)
        {
            var postUp = _mapper.Map<Post>(post);
            await _postCommandRepository.UpdatePostAsync(postUp);
        }
    }
}
