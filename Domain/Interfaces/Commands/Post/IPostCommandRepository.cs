namespace Domain.Interfaces.Commands.Post
{
    public interface IPostCommandRepository
    {
        Task<Entities.Posts.Post> CreatePostAsync(Entities.Posts.Post post);
        Task<Entities.Posts.Post> UpdatePostAsync(Entities.Posts.Post post);
        Task UpdatePostMainImage(Guid postId, string mainImageUrl);
        Task IncreasePostView(Guid postId);

    }
}
