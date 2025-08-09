using Application.Core.MediatR.Requests;
using Application.DTOs.Posts;

namespace Application.Queries.Moderators.Posts.GetPostById;

public class GetPostByIdQuery : ModeratorRequest<PostDto>
{
    public required Guid PostId { get; set; }
}