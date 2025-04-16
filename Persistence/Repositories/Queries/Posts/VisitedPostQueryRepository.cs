using Domain.Entities.Users;
using Domain.Interfaces.Queries.Post;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.Queries.Posts
{
    public class VisitedPostQueryRepository : BaseQueryRepository<BarterlyDbContext>, IVisitedPostQueryRepository
    {
        public VisitedPostQueryRepository(BarterlyDbContext context) : base(context)
        {
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
            return await _context.VisitedPosts.Where(x => x.UserId == userId).Include(x=>x.Post).ToListAsync();
        }
        public async Task<VisitedPost> GetUserVisitedPost(Guid postId, Guid userId)
        {
            return await _context.VisitedPosts.FirstOrDefaultAsync(x => x.PostId == postId && x.UserId == userId);
        }
    }
}
