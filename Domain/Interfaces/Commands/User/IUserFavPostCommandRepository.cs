using Domain.Entities.Users;

namespace Domain.Interfaces.Commands.User
{
    public interface IUserFavPostCommandRepository
    {
        Task CreateUserFavPostAsync(UserFavouritePost userFavPost);
        Task DeleteUserFavPostAsync(Guid id);
    }
}
