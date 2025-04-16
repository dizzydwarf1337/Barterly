using Domain.Entities.Posts;

namespace Domain.Interfaces.Queries.Post
{
    public interface IPostImageQueryRepository
    {
        Task<PostImage> GetPostImageAsync(Guid id);
        Task<ICollection<PostImage>> GetPostImagesAsync();
        Task<ICollection<PostImage>> GetPostImagesByPostIdAsync(Guid postId);
    }
}
