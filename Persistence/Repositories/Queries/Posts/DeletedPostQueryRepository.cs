using Domain.Entities;
using Domain.Interfaces.Queries.Post;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.Queries.Post
{
    public class DeletedPostQueryRepository : BaseQueryRepository<BarterlyDbContext>, IDeletedPostQueryRepository
    {
        public DeletedPostQueryRepository(BarterlyDbContext context) : base(context)
        {
        }

        public async Task<DeletedPost> GetDeletedPostByIdAsync(Guid id)
        {
            return await _context.DeletedPosts.FindAsync(id) ?? throw new Exception("Deleted post not found");
        }

        public async Task<ICollection<DeletedPost>> GetDeletedPostsAsync()
        {
            return await _context.DeletedPosts.ToListAsync();
        }

        public async Task<ICollection<DeletedPost>> GetDeletedPostsByUserIdAsync(Guid userId)
        {
            return await _context.DeletedPosts.Where(x => x.OwnerId == userId).ToListAsync();
        }
    }
}
