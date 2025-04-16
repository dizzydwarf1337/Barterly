using Domain.Entities.Posts;

namespace Domain.Interfaces.Commands.Post
{
    public interface IPostOpinionCommandRepository
    {
        Task CreatePostOpinionAsync(PostOpinion opinion);
        Task UpdatePostOpinionAsync(PostOpinion opinion);
        Task SetHiddenPostOpinionAsync(Guid id, bool value);
        Task DeletePostOpinionAsync(Guid id);
    }
}
