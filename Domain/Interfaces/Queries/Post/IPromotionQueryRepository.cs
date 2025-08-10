using Domain.Entities.Posts;

namespace Domain.Interfaces.Queries.Post;

public interface IPromotionQueryRepository
{
    Task<Promotion> GetPromotionByIdAsync(Guid id, CancellationToken token);
    Task<Promotion> GetPromotionByPostIdAsync(Guid postId, CancellationToken token);
}