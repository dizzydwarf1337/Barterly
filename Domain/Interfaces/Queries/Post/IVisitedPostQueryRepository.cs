using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Queries.Post
{
    public interface IVisitedPostQueryRepository
    {
        Task<VisitedPost> GetVisitedPostByIdAsync(Guid id);
        Task<ICollection<VisitedPost>> GetVisitedPostsAsync();
        Task<ICollection<VisitedPost>> GetVisitedPostsByUserIdAsync(Guid userId);
        Task<ICollection<VisitedPost>> GetVisitedPostsByPostIdAsync(Guid postId);
    }
}
