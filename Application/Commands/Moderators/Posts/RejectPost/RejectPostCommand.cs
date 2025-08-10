using Application.Core.MediatR.Requests;
using MediatR;

namespace Application.Commands.Moderators.Posts.RejectPost;

public class RejectPostCommand : ModeratorRequest<Unit>
{
    public required Guid PostId { get; set; }
    public string Reason { get; set; }
}