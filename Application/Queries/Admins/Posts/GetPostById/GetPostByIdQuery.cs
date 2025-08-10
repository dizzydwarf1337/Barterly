using Application.Core.MediatR.Requests;
using Application.DTOs.Posts;

namespace Application.Queries.Admins.Posts.GetPostById;

public class GetPostByIdQuery : AdminRequest<PostDto>
{
    public required Guid PostId { get; set; }
}