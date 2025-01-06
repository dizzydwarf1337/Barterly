using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Commands.Post;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.Commands.Post
{
    public class PromotionCommandRepository : BaseCommandRepository<BarterlyDbContext>, IPromotionCommandRepository
    {
        public PromotionCommandRepository(BarterlyDbContext context) : base(context)
        {
        }

        public async Task BuyPromotionAsync(Guid id, PostPromotionType type, DateTime endDate)
        {
            var promotion = await _context.Promotions.FindAsync(id) ?? throw new Exception("Promotion not found");
            promotion.Type = type;
            promotion.EndDate = endDate;
            await _context.SaveChangesAsync();
        }

        public async Task CreatePromotionAsync(Promotion promotion)
        {
            await _context.AddAsync(promotion);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePromotionAsync(Guid id)
        {
            var promotion = await _context.Promotions.FindAsync(id) ?? throw new Exception("Promotion not found");
            _context.Remove(promotion);
            await _context.SaveChangesAsync();
        }

        public async Task ExtendPromotionAsync(Guid id, DateTime endDate)
        {
            var promotion = await _context.Promotions.FindAsync(id) ?? throw new Exception("Promotion not found");
            promotion.EndDate = endDate;
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePromotionAsync(Promotion promotion)
        {
            _context.Update(promotion);
            await _context.SaveChangesAsync();
        }
    }
}
