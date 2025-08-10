namespace Domain.Interfaces.Commands.Post;

public interface IVisitedPostCommandRepository
{
    Task UpdateVisitedPost(VisitedPost post, CancellationToken token);
    Task DeleteVisitedPostAsync(Guid id, CancellationToken token);
    Task VisitPost(Guid postId, Guid userId, CancellationToken token);
}