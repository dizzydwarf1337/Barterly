using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces
{
    public interface IPostService
    {
        Task CreatePost(PostDto post);
        Task UpdatePost(PostDto post);
        Task ChangePostHiddenStatus(Guid postId,bool value);
        Task DeletePost(Guid postId);
        Task<PostDto> GetPostById(Guid postId);
        Task<ICollection<PostDto>> GetAllPostsAsync();
        Task<ICollection<PostDto>> GetPostsByUserId(Guid userId);
        Task<ICollection<PostDto>> GetPostsBySubCategoryId(Guid subCategoryId);
        Task<ICollection<PostDto>> GetFavouritePostsBySessionId(Guid sessionId);
        Task<ICollection<PostDto>> GetFavouritePostsByUserId(Guid categoryId);
        Task<ICollection<PostDto>> GetPaginatedPosts(int PageSize, int PageNumber);
        Task<ICollection<PostDto>> GetPaginatedPostsByCategoryId(Guid categoryId, int PageSize, int PageNumber);
        Task<ICollection<PostDto>> GetPaginatedPostsBySubCategoryId(Guid subCategoryId, int PageSize, int PageNumber);
        Task<ICollection<PostDto>> GetPaginatedPostsByUserId(Guid userId, int PageSize, int PageNumber);

    }
}
