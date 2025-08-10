using Application.Core.MediatR.Requests;
using MediatR;

namespace Application.Commands.Moderators.Posts.DeletePost;

public class DeletePostCommand : ModeratorRequest<Unit>
{
    public required Guid PostId { get; set; }
}