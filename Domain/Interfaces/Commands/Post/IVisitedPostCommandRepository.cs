using Domain.Entities.Users;

namespace Domain.Interfaces.Commands.Post
{
    public interface IVisitedPostCommandRepository
    {
        Task CreateVisitedPostAsync(VisitedPost visitedPost);
        Task UpdateVisitedPost(VisitedPost post);
        Task DeleteVisitedPostAsync(Guid id);
    }
}
