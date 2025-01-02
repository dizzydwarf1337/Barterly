using Domain.Entities;
using Domain.Enums;
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
    public class PromotionRepository : BaseRepository, IPromotionCommandRepository, IPromotionQueryRepository
    {
        public PromotionRepository(BarterlyDbContext context) : base(context) { }
        public async Task BuyPromotionAsync(Guid id, PostPromotionType type, DateTime endDate)
        {
            var promotion = await GetPromotionByIdAsync(id);
            promotion.Type= type;
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
            var promotion = await GetPromotionByIdAsync(id);
            _context.Remove(promotion);
            await _context.SaveChangesAsync();
        }

        public async Task ExtendPromotionAsync(Guid id, DateTime endDate)
        {
            var promotion = await GetPromotionByIdAsync(id);
            promotion.EndDate = endDate;
            await _context.SaveChangesAsync();
        }

        public async Task<Promotion> GetPromotionByIdAsync(Guid id)
        {
            return await _context.Promotions.FindAsync(id) ?? throw new Exception("Promotion not found");
        }

        public async Task<Promotion> GetPromotionByPostIdAsync(Guid id)
        {
            return await _context.Promotions.FirstOrDefaultAsync(x=>x.PostId==id) ?? throw new Exception("Post promotion not found");
        }

        public async Task<ICollection<Promotion>> GetPromotionsAsync()
        {
            return await _context.Promotions.ToListAsync();
        }

        public async Task<ICollection<Promotion>> GetPromotionsByTypeAsync(PostPromotionType type)
        {
            return await _context.Promotions.Where(x=>x.Type == type).ToListAsync();
        }

        public async Task UpdatePromotionAsync(Promotion promotion)
        {
            _context.Update(promotion);
            await _context.SaveChangesAsync();
        }
    }
}
