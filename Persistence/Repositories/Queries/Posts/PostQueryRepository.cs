using Domain.Enums;
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
    public class PostQueryRepository : BaseQueryRepository<BarterlyDbContext>, IPostQueryRepository
    {
        public PostQueryRepository(BarterlyDbContext context) : base(context)
        {
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
            return await _context.Posts.Where(x => x.Region.Equals(region, StringComparison.OrdinalIgnoreCase)).ToListAsync();
        }

        public async Task<ICollection<Domain.Entities.Post>> GetPostsBySubCategoryIdAsync(Guid subCategoryId)
        {
            return await _context.Posts.Where(x => x.SubCategoryId == subCategoryId).ToListAsync();
        }

        public async Task<ICollection<Domain.Entities.Post>> GetPromotedPostsAsync()
        {
            return await _context.Posts.Include(x => x.Promotion).Where(x => x.Promotion.Type != PostPromotionType.None).ToListAsync();
        }

        public async Task<ICollection<Domain.Entities.Post>> GetUserPromotedPostsAsync(Guid userId)
        {
            return await _context.Posts.Include(x => x.Owner).Include(x => x.Promotion).Where(x => x.Owner.Id == userId && x.Promotion.Type != PostPromotionType.None).ToListAsync();
        }


    }
}
