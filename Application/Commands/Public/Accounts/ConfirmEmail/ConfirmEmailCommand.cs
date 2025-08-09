using Application.Core.MediatR.Requests;
using MediatR;

namespace Application.Commands.Public.Accounts.ConfirmEmail;

public class ConfirmEmailCommand : PublicRequest<Unit>
{
    public required string userMail { get; set; }
    public required string token { get; set; }
}