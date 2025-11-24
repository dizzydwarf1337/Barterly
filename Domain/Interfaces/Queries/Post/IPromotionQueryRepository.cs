using Domain.Entities.Posts;

namespace Domain.Interfaces.Queries.Post;

public interface IPromotionQueryRepository
{
    IQueryable<Promotion> GetPromotions();
    Task<Promotion> GetPromotionByIdAsync(Guid id, CancellationToken token);
    Task<Promotion> GetPromotionByPostIdAsync(Guid postId, CancellationToken token);
}