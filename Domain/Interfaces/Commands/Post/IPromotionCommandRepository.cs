using Domain.Entities.Posts;
using Domain.Enums;

namespace Domain.Interfaces.Commands.Post
{
    public interface IPromotionCommandRepository
    {
        Task UpdatePromotionAsync(Promotion promotion);
    }
}
