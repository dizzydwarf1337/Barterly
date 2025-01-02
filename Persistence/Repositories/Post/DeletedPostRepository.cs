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
    public class DeletedPostRepository : BaseRepository, IDeletedPostCommandRepository, IDeletedPostQueryRepository
    {
        public DeletedPostRepository(BarterlyDbContext context) : base(context) { }
        public async Task CreateDeletedPostAsync(DeletedPost deletedPost)
        {
            await _context.DeletedPosts.AddAsync(deletedPost);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDeletedPostAsync(Guid id)
        {
            var deletedPost = _context.DeletedPosts.Find(id) ?? throw new Exception("Deleted post not found");
            _context.DeletedPosts.Remove(deletedPost);
            await _context.SaveChangesAsync();
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
