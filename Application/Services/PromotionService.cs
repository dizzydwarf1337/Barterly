using Application.Interfaces;
using Domain.Enums;

namespace Application.Services
{
    public class PromotionService : IPromotionService
    {
        public Task BuyPromotion(Guid ProductId, PostPromotionType postPromotionType, DateTime until)
        {
            throw new NotImplementedException();
        }

        public Task CancelPromotion(Guid PromotionId)
        {
            throw new NotImplementedException();
        }
    }
}
