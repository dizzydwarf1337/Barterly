using API.Core.ApiResponse;
using Application.Interfaces;
using Domain.Exceptions.BusinessExceptions;
using Domain.Exceptions.DataExceptions;
using MediatR;

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
            catch (EntityNotFoundException ex)
            {
                return ApiResponse<Unit>.Failure(ex.Message, 404);
            }
            catch (AccessForbiddenException ex)
            {
                return ApiResponse<Unit>.Failure(ex.Message, 403);
            }
            catch(InvalidDataProvidedException ex)
            {
                return ApiResponse<Unit>.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiResponse<Unit>.Failure(ex.Message + ex.StackTrace);
            }
        }
    }
}
