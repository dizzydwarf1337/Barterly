using Application.Core.MediatR.Requests;
using MediatR;

namespace Application.Commands.Public.Accounts.ResendEmailConfirm;

public class ResendEmailConfirmCommand : PublicRequest<Unit>
{
    public required string Email { get; set; }
}