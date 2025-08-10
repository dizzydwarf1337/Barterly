using Application.Core.MediatR.Requests;
using MediatR;

namespace Application.Commands.Public.Accounts.ConfirmEmail;

public class ConfirmEmailCommand : PublicRequest<Unit>
{
    public required string UserMail { get; set; }
    public required string Token { get; set; }
}