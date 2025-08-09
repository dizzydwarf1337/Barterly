using Application.Core.MediatR.Requests;
using MediatR;

namespace Application.Commands.Public.Accounts.ResetPassword;

public class ResetPasswordCommand : PublicRequest<Unit>
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string Token { get; set; }
}