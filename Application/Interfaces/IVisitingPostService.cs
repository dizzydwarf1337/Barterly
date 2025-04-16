using Domain.Entities.Users;

namespace Application.Interfaces
{
    public interface IVisitingPostService
    {
        Task VisitPost(Guid postId, Guid userId);
        Task<VisitedPost> GetVisitedPostByIdAsync(Guid id);
        Task<ICollection<VisitedPost>> GetVisitedPostsByUserIdAsync(Guid userId);
        Task<ICollection<VisitedPost>> GetVisitedPostsByPostIdAsync(Guid postId);

    }
}
