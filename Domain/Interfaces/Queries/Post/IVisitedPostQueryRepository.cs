namespace Domain.Interfaces.Queries.Post;

public interface IVisitedPostQueryRepository
{
    Task<VisitedPost> GetVisitedPostByIdAsync(Guid id, CancellationToken token);
    IQueryable<VisitedPost> GetVisitedPostsAsync();
    Task<ICollection<VisitedPost>> GetVisitedPostsByUserIdAsync(Guid userId, CancellationToken token);
    Task<ICollection<VisitedPost>> GetVisitedPostsByPostIdAsync(Guid postId, CancellationToken token);
    Task<VisitedPost> GetUserVisitedPost(Guid postId, Guid userId, CancellationToken token);
}