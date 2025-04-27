using Domain.Entities.Posts;
using Domain.Enums;

namespace Domain.Interfaces.Queries.Post
{
    public interface IPromotionQueryRepository
    {
        Task<Promotion> GetPromotionByIdAsync(Guid id);
        Task<Promotion> GetPromotionByPostIdAsync(Guid postId);

    }
}
