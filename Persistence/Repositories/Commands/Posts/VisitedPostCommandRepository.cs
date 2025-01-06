using Domain.Entities;
using Domain.Interfaces.Commands.Post;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.Commands.Post
{
    public class VisitedPostCommandRepository : BaseCommandRepository<BarterlyDbContext>, IVisitedPostCommandRepository
    {
        public VisitedPostCommandRepository(BarterlyDbContext context) : base(context)
        {
        }

        public async Task CreateVisitedPostAsync(VisitedPost visitedPost)
        {
            await _context.VisitedPosts.AddAsync(visitedPost);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteVisitedPostAsync(Guid id)
        {
            var visitedPost = await _context.VisitedPosts.FindAsync(id) ?? throw new Exception("VisitedPost not found");
            _context.VisitedPosts.Remove(visitedPost);
            await _context.SaveChangesAsync();
        }

        public async Task IncreaseVisitedCount(Guid id)
        {
            var visitedPost = await _context.VisitedPosts.FindAsync(id) ?? throw new Exception("VisitedPost not found");
            visitedPost.VisitedCount++;
        }

        public async Task UpdateLastVisited(Guid id, DateTime date)
        {
            var visitedPost = await _context.VisitedPosts.FindAsync(id) ?? throw new Exception("VisitedPost not found");
            visitedPost.LastVisitedAt = date;
            await _context.SaveChangesAsync();

        }
    }
}
