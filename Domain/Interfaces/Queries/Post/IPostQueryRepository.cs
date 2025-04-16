using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces.Queries.Post
{
    public interface IPostQueryRepository
    {
        Task<Entities.Posts.Post> GetPostByIdAsync(Guid postId);
        Task<ICollection<Entities.Posts.Post>> GetAllPostsAsync();
        Task<ICollection<Entities.Posts.Post>> GetPostsByOwnerIdAsync(Guid ownerId);
        Task<ICollection<Entities.Posts.Post>> GetFiltredPostsAsync(int? pageCount, int? page, Guid? categoryId = null, Guid? subCategoryId = null, string? city = null, string? region = null);
        Task<ICollection<Entities.Posts.Post>> GetPostsByCreatedAtAsync(DateTime createdAt);
        Task<ICollection<Entities.Posts.Post>> GetUserPromotedPostsAsync(Guid userId);
        Task<ICollection<Entities.Posts.Post>> GetUserFavouritePosts(Guid userId);
        Task<ICollection<Entities.Posts.Post>> GetFeed(string categories, string cities, int pageCount, int Page);
        Task<ICollection<Entities.Posts.Post>> GetPromotedPosts(int count, string? categories, string? cities);
    }
}
