using Application.Core.MediatR.Requests;
using MediatR;

namespace Application.Commands.Moderators.Posts.ApprovePost;

public class ApprovePostCommand : ModeratorRequest<Unit>
{
    public Guid PostId { get; set; }
}