using Domain.Entities;
using Domain.Interfaces.Commands.Post;
using Domain.Interfaces.Queries.Post;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.Post
{
    public class PostImageRepository : BaseRepository, IPostImageCommandRepository, IPostImageQueryRepository
    {
        public PostImageRepository(BarterlyDbContext context) : base(context) { }
        public async Task CreatePostImageAsync(PostImage postImage)
        {
            await _context.PostImages.AddAsync(postImage);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePostImageAsync(Guid id)
        {
            var postImage = await GetPostImageAsync(id);
            _context.PostImages.Remove(postImage);
            await _context.SaveChangesAsync();
        }

        public async Task<PostImage> GetPostImageAsync(Guid id)
        {
            return await _context.PostImages.FindAsync(id) ?? throw new Exception("Post image not found");
        }

        public async Task<ICollection<PostImage>> GetPostImagesAsync()
        {
            return await _context.PostImages.AsNoTracking().ToListAsync();
        }

        public async Task<ICollection<PostImage>> GetPostImagesByPostIdAsync(Guid postId)
        {
            return await _context.PostImages.Where(x => x.PostId == postId).AsNoTracking().ToListAsync();
        }
    }
}
