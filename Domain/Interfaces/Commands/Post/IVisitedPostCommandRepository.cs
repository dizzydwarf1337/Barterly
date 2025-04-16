using Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Commands.Post
{
    public interface IVisitedPostCommandRepository
    {
        Task CreateVisitedPostAsync(VisitedPost visitedPost);
        Task UpdateVisitedPost(VisitedPost post);
        Task DeleteVisitedPostAsync(Guid id);
    }
}
