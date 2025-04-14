using API.Core.ApiResponse;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Commands.ConfirmEmail
{
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, ApiResponse<Unit>>
    {
        private readonly IUserService _userService;
        private readonly IMailService _mailService;

        public ConfirmEmailCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<ApiResponse<Unit>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _userService.ConfirmEmail(request.userMail, request.token);

                return ApiResponse<Unit>.Success(Unit.Value);
            }
            catch (Exception ex) {
                return ApiResponse<Unit>.Failure(ex.Message+ex.StackTrace);
            }
        }
    }
}
