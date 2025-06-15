using API.Core.ApiResponse;
using Application.Interfaces;
using Domain.Entities.Users;
using Domain.Enums.Common;
using Domain.Exceptions.BusinessExceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;

namespace Application.Features.Users.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ApiResponse<Unit>>
    {
        private readonly UserManager<User> _userManager;

        public ResetPasswordCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApiResponse<Unit>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {

            var user = await _userManager.FindByEmailAsync(request.Email) ?? throw new EntityNotFoundException("User");
            var res = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);
            return ApiResponse<Unit>.Success(Unit.Value);
            
        }
    }
}
