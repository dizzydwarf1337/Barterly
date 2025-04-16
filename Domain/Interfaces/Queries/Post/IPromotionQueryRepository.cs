using Domain.Entities.Posts;
using Domain.Enums;

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
