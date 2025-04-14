using API.Core.ApiResponse;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ApiResponse<Unit>>
    {
        private readonly IAuthService _authService;

        public CreateUserCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }


        async Task<ApiResponse<Unit>> IRequestHandler<CreateUserCommand, ApiResponse<Unit>>.Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _authService.Register(request.registerDto);
                return ApiResponse<Unit>.Success(Unit.Value);
            }
            catch (Exception ex)
            {
                return ApiResponse<Unit>.Failure($"{ex.Message+ex.StackTrace}");
            }
        }
    }
}
