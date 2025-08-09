using Domain.Entities.Posts;

namespace Domain.Interfaces.Queries.Post;

public interface IPostImageQueryRepository
{
    Task<PostImage> GetPostImageAsync(Guid id, CancellationToken token);
    Task<ICollection<PostImage>> GetPostImagesAsync(CancellationToken token);
    Task<ICollection<PostImage>> GetPostImagesByPostIdAsync(Guid postId, CancellationToken token);
}