using Application.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces
{
    public interface IUserService
    {
        Task RegisteUser(RegisterDto registerDto);
        Task DeleteUser(Guid userId);
        Task UpdateUser(UserDto userDto);

        Task<User> GetUserById(Guid userId);
    }
}
