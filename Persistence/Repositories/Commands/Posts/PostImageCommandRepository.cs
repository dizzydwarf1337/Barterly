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
    public class PostImageCommandRepository : BaseCommandRepository<BarterlyDbContext>, IPostImageCommandRepository
    {
        public PostImageCommandRepository(BarterlyDbContext context) : base(context)
        {
        }

        public async Task CreatePostImageAsync(PostImage postImage)
        {
            await _context.PostImages.AddAsync(postImage);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePostImageAsync(Guid id)
        {
            var postImage = await _context.PostImages.FindAsync(id);
            _context.PostImages.Remove(postImage);
            await _context.SaveChangesAsync();
        }
    }
}
