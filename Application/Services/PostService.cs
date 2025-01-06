using Application.DTOs;
using Application.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    internal class PostService : IPostService
    {
        public Task ChangePostHiddenStatus(Guid postId, bool value)
        {
            throw new NotImplementedException();
        }

        public Task CreatePost(PostDto post)
        {
            throw new NotImplementedException();
        }

        public Task DeletePost(Guid postId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<PostDto>> GetAllPostsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<PostDto>> GetFavouritePostsBySessionId(Guid sessionId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<PostDto>> GetFavouritePostsByUserId(Guid categoryId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<PostDto>> GetPaginatedPosts(int PageSize, int PageNumber)
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

        public Task<PostDto> GetPostById(Guid postId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<PostDto>> GetPostsBySubCategoryId(Guid subCategoryId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<PostDto>> GetPostsByUserId(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task UpdatePost(PostDto post)
        {
            throw new NotImplementedException();
        }
    }
}
