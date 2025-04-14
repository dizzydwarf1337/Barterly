using API.Core.ApiResponse;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ApiResponse<Unit>>
    {
        private readonly IUserService _userService;

        public ResetPasswordCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<ApiResponse<Unit>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _userService.ResetPassword(request.Email, request.Token, request.Password);
                return ApiResponse<Unit>.Success(Unit.Value);
            }
            catch (Exception ex) 
            {
                return ApiResponse<Unit>.Failure($"Error while resetting password, {ex}");
            }
        }
    }
}
