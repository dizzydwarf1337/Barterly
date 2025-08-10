using Domain.Entities.Users;

namespace Domain.Interfaces.Commands.User;

public interface IUserFavPostCommandRepository
{
    Task CreateUserFavPostAsync(UserFavouritePost userFavPost, CancellationToken token);
    Task DeleteUserFavPostAsync(Guid id, CancellationToken token);
}