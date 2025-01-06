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
    public class DeletedPostCommandRepository : BaseCommandRepository<BarterlyDbContext>, IDeletedPostCommandRepository
    {
        public DeletedPostCommandRepository(BarterlyDbContext context) : base(context)
        {
        }

        public async Task CreateDeletedPostAsync(DeletedPost deletedPost)
        {
            await _context.DeletedPosts.AddAsync(deletedPost);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDeletedPostAsync(Guid id)
        {
            var deletedPost = await _context.DeletedPosts.FindAsync(id) ?? throw new Exception("Deleted post not found");
            _context.DeletedPosts.Remove(deletedPost);
            await _context.SaveChangesAsync();
        }
    }
}
