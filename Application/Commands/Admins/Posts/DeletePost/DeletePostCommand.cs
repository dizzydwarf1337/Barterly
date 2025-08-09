using Application.Core.MediatR.Requests;
using MediatR;

namespace Application.Commands.Admins.Posts.DeletePost;

public class DeletePostCommand : AdminRequest<Unit>
{
    public required Guid PostId { get; set; }
}