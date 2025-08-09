using Application.Core.MediatR.Requests;
using Domain.Enums.Users;
using MediatR;

namespace Application.Commands.Admins.Users.CreateUser;

public class CreateUserCommand : AdminRequest<Unit>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public UserRoles Role { get; set; }
}