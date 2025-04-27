using Domain.Entities.Posts;
using Domain.Enums.Posts;

namespace Application.Interfaces
{
    public interface IPromotionService
    {
        Task BuyPromotion(Guid postId, PostPromotionType postPromotionType, DateTime until);
        Task CancelPromotion(Guid PromotionId);
        Task ExtendPromotion(Guid promotionId, DateTime until);
    }
}
