using API.Core.ApiResponse;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Commands.ResendEmailConfirm
{
    public class ResendEmailConfirmCommandHandler : IRequestHandler<ResendEmailConfirmCommand, ApiResponse<Unit>>
    {
        private readonly IMailService _mailService;
        private readonly ITokenService _tokenService;

        public ResendEmailConfirmCommandHandler(IMailService mailService, ITokenService tokenService)
        {
            _mailService = mailService;
            _tokenService = tokenService;
        }

        public async Task<ApiResponse<Unit>> Handle(ResendEmailConfirmCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _tokenService.DeleteTokenByUserMail(request.Email, Domain.Enums.TokenType.EmailConfirmation);
                await _mailService.SendConfiramationMail(request.Email);
                return ApiResponse<Unit>.Success(value:Unit.Value);
            }
            catch
            {
                return ApiResponse<Unit>.Failure("Error while resenging email confirmation");
            }
        }
    }
}
