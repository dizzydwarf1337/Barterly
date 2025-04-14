using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IVisitingPostService
    {
        Task VisitPost(Guid postId, Guid userId);
        Task<VisitedPost> GetVisitedPostByIdAsync(Guid id);
        Task<ICollection<VisitedPost>> GetVisitedPostsByUserIdAsync(Guid userId);
        Task<ICollection<VisitedPost>> GetVisitedPostsByPostIdAsync(Guid postId);

    }
}
