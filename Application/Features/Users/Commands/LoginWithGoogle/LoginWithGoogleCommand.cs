using API.Core.ApiResponse;
using Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Commands.LoginWithGoogle
{
    public class LoginWithGoogleCommand : IRequest<ApiResponse<UserDto>>
    {
        public GoogleLoginDto googleLoginDto { get; set; }
    }
}
