using Domain.Entities.Posts;

namespace Domain.Interfaces.Commands.Post;

public interface IPromotionCommandRepository
{
    Task UpdatePromotionAsync(Promotion promotion, CancellationToken token);
}