using Domain.Entities;
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
        Task UpdateLastVisited(Guid id, DateTime date);
        Task IncreaseVisitedCount(Guid id);
        Task DeleteVisitedPostAsync(Guid id);
    }
}
