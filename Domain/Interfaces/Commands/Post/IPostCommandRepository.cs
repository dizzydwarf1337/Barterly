namespace Domain.Interfaces.Commands.Post
{
    public interface IPostCommandRepository
    {
        Task<Entities.Posts.Post> CreatePostAsync(Entities.Posts.Post post);
        Task UpdatePostAsync(Entities.Posts.Post post);

    }
}
