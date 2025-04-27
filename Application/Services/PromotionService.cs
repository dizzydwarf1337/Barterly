using Application.Interfaces;
using Domain.Enums.Posts;
using Domain.Interfaces.Commands.Post;
using Domain.Interfaces.Queries.Post;

namespace Application.Services
{
    public class PromotionService : IPromotionService
    {
        private readonly IPromotionQueryRepository _promotionQueryRepository;
        private readonly IPromotionCommandRepository _promotionCommandRepository;

        public PromotionService(IPromotionQueryRepository promotionQueryRepository, IPromotionCommandRepository promotionCommandRepository)
        {
            _promotionQueryRepository = promotionQueryRepository;
            _promotionCommandRepository = promotionCommandRepository;
        }

        public async Task BuyPromotion(Guid postId, PostPromotionType postPromotionType, DateTime until)
        {
            var promotion = await _promotionQueryRepository.GetPromotionByPostIdAsync(postId);
            promotion.Type = postPromotionType;
            promotion.EndDate = until;
            await _promotionCommandRepository.UpdatePromotionAsync(promotion);
        }

        public async Task CancelPromotion(Guid PromotionId)
        {
            var promotion = await _promotionQueryRepository.GetPromotionByIdAsync(PromotionId);
            promotion.Type = PostPromotionType.None;
            promotion.EndDate = DateTime.MaxValue;
            await _promotionCommandRepository.UpdatePromotionAsync(promotion);
        }

        public async Task ExtendPromotion(Guid promotionId, DateTime until)
        {
            var promotion = await _promotionQueryRepository.GetPromotionByIdAsync(promotionId);
            promotion.EndDate = until;
            await _promotionCommandRepository.UpdatePromotionAsync(promotion);
        }
    }
}
