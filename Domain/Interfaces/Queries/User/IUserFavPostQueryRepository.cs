using Domain.Entities.Users;

namespace Domain.Interfaces.Queries.User
{
    public interface IUserFavPostQueryRepository
    {
        Task<ICollection<UserFavouritePost>> GetUserFavPostsAsync();
        Task<UserFavouritePost> GetUserFavPostByIdAsync(Guid id);
        Task<ICollection<UserFavouritePost>> GetUserFavPostsByUserIdAsync(Guid userId);
    }
}
