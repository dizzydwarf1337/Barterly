using Domain.Entities.Posts;
using Domain.Enums;
using Domain.Interfaces.Queries.Post;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories.Queries.Post
{
    public class PromotionQueryRepository : BaseQueryRepository<BarterlyDbContext>, IPromotionQueryRepository
    {
        public PromotionQueryRepository(BarterlyDbContext context) : base(context)
        {
        }

        public async Task<Promotion> GetPromotionByIdAsync(Guid id)
        {
            return await _context.Promotions.FindAsync(id) ?? throw new Exception("Promotion not found");
        }

        public async Task<Promotion> GetPromotionByPostIdAsync(Guid id)
        {
            return await _context.Promotions.FirstOrDefaultAsync(x => x.PostId == id) ?? throw new Exception("Post promotion not found");
        }

        public async Task<ICollection<Promotion>> GetPromotionsAsync()
        {
            return await _context.Promotions.ToListAsync();
        }

        public async Task<ICollection<Promotion>> GetPromotionsByTypeAsync(PostPromotionType type)
        {
            return await _context.Promotions.Where(x => x.Type == type).ToListAsync();
        }

    }
}
