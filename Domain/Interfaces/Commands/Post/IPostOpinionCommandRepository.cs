using Domain.Entities.Posts;

namespace Domain.Interfaces.Commands.Post;

public interface IPostOpinionCommandRepository
{
    Task<PostOpinion> CreatePostOpinionAsync(PostOpinion opinion, CancellationToken token);
    Task<PostOpinion> UpdatePostOpinionAsync(Guid id, string content, int rate, CancellationToken token);
    Task SetHiddenPostOpinionAsync(Guid id, bool value, CancellationToken token);
    Task DeletePostOpinionAsync(Guid id, CancellationToken token);
}