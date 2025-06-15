using Domain.Entities.Posts;

namespace Domain.Interfaces.Commands.Post
{
    public interface IPostOpinionCommandRepository
    {
        Task<PostOpinion> CreatePostOpinionAsync(PostOpinion opinion);
        Task<PostOpinion> UpdatePostOpinionAsync(Guid id, string content,int rate);
        Task SetHiddenPostOpinionAsync(Guid id, bool value);
        Task DeletePostOpinionAsync(Guid id);
    }
}
