using Application.DTOs;
using Application.ServiceInterfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService : IUserService
    {
        public Task DeleteUser(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserById(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task RegisteUser(RegisterDto registerDto)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUser(UserDto userDto)
        {
            throw new NotImplementedException();
        }
    }
}
