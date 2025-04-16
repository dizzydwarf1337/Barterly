using Domain.Entities.Posts;
using Domain.Enums;

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
