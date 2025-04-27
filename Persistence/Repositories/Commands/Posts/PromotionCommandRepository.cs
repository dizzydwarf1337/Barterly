using Domain.Entities.Posts;
using Domain.Enums;
using Domain.Interfaces.Commands.Post;
using Persistence.Database;

namespace Persistence.Repositories.Commands.Post
{
    public class PromotionCommandRepository : BaseCommandRepository<BarterlyDbContext>, IPromotionCommandRepository
    {
        public PromotionCommandRepository(BarterlyDbContext context) : base(context)
        {
        }

        public async Task UpdatePromotionAsync(Promotion promotion)
        {
            _context.Promotions.Update(promotion);
            await _context.SaveChangesAsync();
        }
    }
}
