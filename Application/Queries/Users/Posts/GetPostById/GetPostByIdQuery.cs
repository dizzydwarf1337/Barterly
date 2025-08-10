using Application.Core.MediatR.Requests;
using Application.DTOs.Posts;

namespace Application.Queries.Users.Posts.GetPostById;

public class GetPostByIdQuery : UserRequest<PostDto>
{
    public Guid PostId { get; set; }
}