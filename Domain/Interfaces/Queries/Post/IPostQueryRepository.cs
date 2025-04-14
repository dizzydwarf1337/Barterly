using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces.Queries.Post
{
    public interface IPostQueryRepository
    {
        Task<Domain.Entities.Post> GetPostByIdAsync(Guid postId);
        Task<ICollection<Domain.Entities.Post>> GetAllPostsAsync();
        Task<ICollection<Domain.Entities.Post>> GetPostsByOwnerIdAsync(Guid ownerId);
        Task<ICollection<Domain.Entities.Post>> GetFiltredPostsAsync(int? pageCount, int? page, Guid? categoryId = null, Guid? subCategoryId = null, string? city = null, string? region = null);
        Task<ICollection<Domain.Entities.Post>> GetPostsByCreatedAtAsync(DateTime createdAt);
        Task<ICollection<Domain.Entities.Post>> GetUserPromotedPostsAsync(Guid userId);
        Task<ICollection<Domain.Entities.Post>> GetUserFavouritePosts(Guid userId);
        Task<ICollection<Domain.Entities.Post>> GetFeed(string categories, string cities, int pageCount, int Page);
        Task<ICollection<Domain.Entities.Post>> GetPromotedPosts(int count, string? categories, string? cities);
    }
}
