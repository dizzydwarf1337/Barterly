namespace Domain.Interfaces.Commands.Post;

public interface IPostCommandRepository
{
    Task<Entities.Posts.Post> CreatePostAsync(Entities.Posts.Post post, CancellationToken token);
    Task<Entities.Posts.Post> UpdatePostAsync(Entities.Posts.Post post, CancellationToken token);
    Task UpdatePostMainImage(Guid postId, string mainImageUrl, CancellationToken token);
    Task IncreasePostView(Guid postId, CancellationToken token);
}