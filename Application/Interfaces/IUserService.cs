using Application.DTOs.User;
using Domain.Entities.Users;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task DeleteUser(Guid userId);
        Task UpdateUser(UserDto userDto);
        Task ConfirmEmail(string email, string token);
        Task ResetPassword(string email, string token, string password);
        Task<User> GetUserById(Guid userId);
    }
}
