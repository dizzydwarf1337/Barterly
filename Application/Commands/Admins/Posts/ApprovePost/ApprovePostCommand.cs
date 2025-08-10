using Application.Core.MediatR.Requests;
using MediatR;

namespace Application.Commands.Admins.Posts.ApprovePost;

public class ApprovePostCommand : AdminRequest<Unit>
{
    public Guid PostId;
}