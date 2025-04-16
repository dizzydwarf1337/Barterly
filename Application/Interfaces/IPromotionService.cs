using Domain.Enums;

namespace Application.Interfaces
{
    public interface IPromotionService
    {
        Task BuyPromotion(Guid ProductId, PostPromotionType postPromotionType, DateTime until);
        Task CancelPromotion(Guid PromotionId);
    }
}
