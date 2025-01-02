using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Queries.Post
{
    public interface IPromotionQueryRepository
    {
        Task<Promotion> GetPromotionByIdAsync(Guid id);
        Task<ICollection<Promotion>> GetPromotionsAsync();
        Task<Promotion> GetPromotionByPostIdAsync(Guid id);
        Task<ICollection<Promotion>> GetPromotionsByTypeAsync(PostPromotionType type);
    }
}
