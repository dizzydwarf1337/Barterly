using Domain.Enums;
using Domain.Interfaces.Commands.Post;
using Domain.Interfaces.Queries.Post;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.Post
{
    public class PostRepository : BaseRepository, IPostCommandRepository, IPostQueryRepository
    {
        public PostRepository(BarterlyDbContext context) : base(context) { }

        public async Task CreatePostAsync(Domain.Entities.Post post)
        {
           await _context.AddAsync(post);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePostAsync(Guid postId)
        {
            var post = await _context.Posts.FindAsync(postId) ?? throw new Exception("Post with this id doesn't exists");
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<Domain.Entities.Post>> GetAllPostsAsync()
        {
            return await _context.Posts.ToListAsync();
        }

        public async Task<Domain.Entities.Post> GetPostByIdAsync(Guid postId)
        {
            return await _context.Posts.FindAsync(postId) ?? throw new Exception("Post with this id doesn't exists");
        }

        public async Task<ICollection<Domain.Entities.Post>> GetPostsByCityAsync(string city)
        {
            return await _context.Posts.Where(x => x.City.Equals(city, StringComparison.OrdinalIgnoreCase)).ToListAsync();
        }

        public async Task<ICollection<Domain.Entities.Post>> GetPostsByCreatedAtAsync(DateTime createdAt)
        {
            return await _context.Posts.Where(x => DateTime.Compare(x.CreatedAt, createdAt) > 0).ToListAsync();
        }

        public async Task<ICollection<Domain.Entities.Post>> GetPostsByOwnerIdAsync(Guid ownerId)
        {
            return await _context.Posts.Where(x => x.OwnerId == ownerId).ToListAsync();
        }

        public async Task<ICollection<Domain.Entities.Post>> GetPostsByRegionAsync(string region)
        {
            return await _context.Posts.Where(x => x.Region.Equals(region,StringComparison.OrdinalIgnoreCase)).ToListAsync();
        }

        public async Task<ICollection<Domain.Entities.Post>> GetPostsBySubCategoryIdAsync(Guid subCategoryId)
        {
           return await _context.Posts.Where(x => x.SubCategoryId == subCategoryId).ToListAsync();
        }

        public async Task<ICollection<Domain.Entities.Post>> GetPromotedPostsAsync()
        {
            return await _context.Posts.Include(x=>x.Promotion).Where(x => x.Promotion.Type != PostPromotionType.None).ToListAsync();
        }

        public async Task<ICollection<Domain.Entities.Post>> GetUserPromotedPostsAsync(Guid userId)
        {
            return await _context.Posts.Include(x => x.Owner).Include(x=>x.Promotion).Where(x=>x.Owner.Id == userId && x.Promotion.Type != PostPromotionType.None).ToListAsync();
        }

        public async Task SetHidePostAsync(Guid postId, bool IsHidden)
        {
           var post = await _context.Posts.FindAsync(postId) ?? throw new Exception("Post with this id doesn't exists");
            post.IsHidden = IsHidden;
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePostAsync(Domain.Entities.Post post)
        {
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
        }
    }
}
