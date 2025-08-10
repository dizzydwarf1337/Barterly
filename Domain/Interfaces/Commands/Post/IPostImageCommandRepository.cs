using Domain.Entities.Posts;

namespace Domain.Interfaces.Commands.Post;

public interface IPostImageCommandRepository
{
    Task CreatePostImageAsync(PostImage postImage, CancellationToken token);
    Task DeletePostImageAsync(Guid id, CancellationToken token);
    Task DeletePostImagesByPostIdAsync(Guid postId, CancellationToken token);
}