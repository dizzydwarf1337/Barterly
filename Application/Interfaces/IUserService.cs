using Application.DTOs.User;
using Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task DeleteUser(Guid userId);
        Task UpdateUser(UserDto userDto);
        Task ConfirmEmail(string email, string token);
        Task ResetPassword(string email, string token,string password);
        Task<User> GetUserById(Guid userId);
    }
}
