using Domain.Entities.Posts;

namespace Domain.Interfaces.Queries.Post;

public interface IPostOpinionQueryRepository
{
    Task<PostOpinion> GetPostOpinionByIdAsync(Guid id, CancellationToken token);
    Task<ICollection<PostOpinion>> GetPostOpinionsByPostIdAsync(Guid postId, CancellationToken token);
    Task<ICollection<PostOpinion>> GetPostOpinionsByAuthorIdAsync(Guid userId, CancellationToken token);
    IQueryable<PostOpinion> GetPostOpinionsAsync();
}