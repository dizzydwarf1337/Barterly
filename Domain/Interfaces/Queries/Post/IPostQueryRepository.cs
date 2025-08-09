namespace Domain.Interfaces.Queries.Post;

public interface IPostQueryRepository
{
    Task<Entities.Posts.Post> GetPostById(Guid postId, CancellationToken token);
    IQueryable<Entities.Posts.Post> GetAllPosts();
}