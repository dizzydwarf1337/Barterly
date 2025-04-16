using Domain.Entities.Posts;

namespace Domain.Interfaces.Commands.Post
{
    public interface IPostImageCommandRepository
    {
        Task CreatePostImageAsync(PostImage postImage);
        Task DeletePostImageAsync(Guid id);
    }
}
