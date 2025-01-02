using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Queries.Post
{
    public interface IDeletedPostQueryRepository
    {
        Task<DeletedPost> GetDeletedPostByIdAsync(Guid id);
        Task<ICollection<DeletedPost>> GetDeletedPostsAsync();
        Task<ICollection<DeletedPost>> GetDeletedPostsByUserIdAsync(Guid userId);
    }
}
