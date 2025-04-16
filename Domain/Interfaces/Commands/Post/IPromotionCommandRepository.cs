using Domain.Entities.Posts;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Commands.Post
{
    public interface IPromotionCommandRepository
    {
        Task CreatePromotionAsync(Promotion promotion);
        Task UpdatePromotionAsync(Promotion promotion);
        Task ExtendPromotionAsync(Guid id, DateTime endDate);
        Task BuyPromotionAsync(Guid id, PostPromotionType type, DateTime endDate);
        Task DeletePromotionAsync(Guid id);
    }
}
