using API.Core.ApiResponse;
using Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Commands.Login
{
    public class LoginCommand : IRequest<ApiResponse<UserDto>>
    {
        public LoginDto loginDto { get; set; }
    }
}
