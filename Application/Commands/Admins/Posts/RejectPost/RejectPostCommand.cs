using Application.Core.MediatR.Requests;
using MediatR;

namespace Application.Commands.Admins.Posts.RejectPost;

public class RejectPostCommand : AdminRequest<Unit>
{
    public Guid PostId { get; set; }
    public string Reason { get; set; }
}