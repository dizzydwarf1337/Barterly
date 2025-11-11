using Domain.Entities.Users;

namespace Domain.Interfaces.Queries.User;

public interface IUserFavPostQueryRepository
{
    IQueryable<UserFavouritePost> GetUserFavPostsAsync();
    Task<UserFavouritePost> GetUserFavPostByIdAsync(Guid postId, Guid userId, CancellationToken token);
    Task<ICollection<UserFavouritePost>> GetUserFavPostsByUserIdAsync(Guid userId, CancellationToken token);
}