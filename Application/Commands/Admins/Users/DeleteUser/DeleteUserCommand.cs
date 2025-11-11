using Application.Core.MediatR.Requests;
using MediatR;

namespace Application.Commands.Admins.Users.DeleteUser;

public class DeleteUserCommand : AdminRequest<Unit>
{
    public Guid Id { get; set; }
}