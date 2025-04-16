using Domain.Entities.Users;

namespace Domain.Interfaces.Queries.Post
{
    public interface IVisitedPostQueryRepository
    {
        Task<VisitedPost> GetVisitedPostByIdAsync(Guid id);
        Task<ICollection<VisitedPost>> GetVisitedPostsAsync();
        Task<ICollection<VisitedPost>> GetVisitedPostsByUserIdAsync(Guid userId);
        Task<ICollection<VisitedPost>> GetVisitedPostsByPostIdAsync(Guid postId);
        Task<VisitedPost> GetUserVisitedPost(Guid postId, Guid userId);
    }
}
