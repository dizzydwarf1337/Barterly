using API.Core.ApiResponse;
using Application.DTOs.Auth;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<ApiResponse<Unit>>
    {
        public RegisterDto registerDto;
    }
}
