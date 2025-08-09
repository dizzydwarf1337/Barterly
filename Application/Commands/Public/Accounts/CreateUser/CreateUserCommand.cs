using Application.Core.MediatR.Requests;
using MediatR;

namespace Application.Commands.Public.Accounts.CreateUser;

public class CreateUserCommand : PublicRequest<Unit>
{
    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required string Email { get; set; }

    public required string Password { get; set; }
}