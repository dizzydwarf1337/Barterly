using Domain.Entities.Users;

namespace Domain.Interfaces.Commands.Post
{
    public interface IVisitedPostCommandRepository
    {
        Task UpdateVisitedPost(VisitedPost post);
        Task DeleteVisitedPostAsync(Guid id);
        Task VisitPost(Guid postId, Guid userId);
    }
}
