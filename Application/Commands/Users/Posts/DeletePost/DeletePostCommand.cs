using Application.Core.MediatR.Requests;
using Application.Interfaces.CommandInterfaces;
using MediatR;

namespace Application.Commands.Users.Posts.DeletePost;

public class DeletePostCommand : UserRequest<Unit>, IPostOwner
{
    public required Guid PostId { get; set; }
}