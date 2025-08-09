using Application.Core.MediatR.Requests;
using Application.DTOs.Posts;

namespace Application.Queries.Public.Posts.GetPostById;

public class GetPostByIdQuery : PublicRequest<PostDto>
{
    public Guid PostId { get; set; }
}