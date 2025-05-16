using Application.DTOs.Auth;
using Application.Features.Users.Commands.CreateUser;
using Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.Factories.Interfaces
{
    public interface IUserFactory
    {
        public Task<User> CreateUser(RegisterDto registerDto);
    }
}
