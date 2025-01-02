using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Queries.Post
{
    public interface IPostQueryRepository
    {
        Task<Domain.Entities.Post> GetPostByIdAsync(Guid postId);
        Task<ICollection<Domain.Entities.Post>> GetAllPostsAsync();
        Task<ICollection<Domain.Entities.Post>> GetPostsBySubCategoryIdAsync(Guid subCategoryId);
        Task<ICollection<Domain.Entities.Post>> GetPostsByOwnerIdAsync(Guid ownerId);
        Task<ICollection<Domain.Entities.Post>> GetPostsByCityAsync(string city);
        Task<ICollection<Domain.Entities.Post>> GetPostsByRegionAsync(string region);
        Task<ICollection<Domain.Entities.Post>> GetPostsByCreatedAtAsync(DateTime createdAt);
        Task<ICollection<Domain.Entities.Post>> GetPromotedPostsAsync();
        Task<ICollection<Domain.Entities.Post>> GetUserPromotedPostsAsync(Guid userId);
    }
}
