using Application.Core.MediatR.Requests;
using MediatR;

namespace Application.Commands.Users.Posts.UpdatePostVisibility;

public class UpdatePostVisibilityCommand : UserRequest<Unit>
{
    public Guid PostId { get; set; }
}