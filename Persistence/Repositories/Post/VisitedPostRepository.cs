using Domain.Entities;
using Domain.Interfaces.Commands.Post;
using Domain.Interfaces.Queries.Post;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.Post
{
    public class VisitedPostRepository : BaseRepository, IVisitedPostCommandRepository, IVisitedPostQueryRepository
    {
        public VisitedPostRepository(BarterlyDbContext context) : base(context) { }
        public async Task CreateVisitedPostAsync(VisitedPost visitedPost)
        {
            await _context.VisitedPosts.AddAsync(visitedPost);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteVisitedPostAsync(Guid id)
        {
            var visitedPost = await GetVisitedPostByIdAsync(id);
            _context.VisitedPosts.Remove(visitedPost);
            await _context.SaveChangesAsync();
        }

        public async Task<VisitedPost> GetVisitedPostByIdAsync(Guid id)
        {
            return await _context.VisitedPosts.FindAsync(id) ?? throw new Exception("Visited post not found");
        }

        public async Task<ICollection<VisitedPost>> GetVisitedPostsAsync()
        {
            return await _context.VisitedPosts.ToListAsync();
        }

        public async Task<ICollection<VisitedPost>> GetVisitedPostsByPostIdAsync(Guid postId)
        {
            return await _context.VisitedPosts.Where(x => x.PostId == postId).ToListAsync();
        }

        public async Task<ICollection<VisitedPost>> GetVisitedPostsByUserIdAsync(Guid userId)
        {
            return await _context.VisitedPosts.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task IncreaseVisitedCount(Guid id)
        {
            var visitedPost = await  GetVisitedPostByIdAsync(id);
            visitedPost.VisitedCount++;
        }

        public async Task UpdateLastVisited(Guid id, DateTime date)
        {
            var visitedPost = await GetVisitedPostByIdAsync(id);
            visitedPost.LastVisitedAt = date;
            await _context.SaveChangesAsync();

        }
    }
}
