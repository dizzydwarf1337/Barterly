using Application.Core.MediatR.Requests;
using Application.DTOs.Posts;

namespace Application.Queries.Users.Posts.GetPostPreviewById;

public class GetPostPreviewByIdQuery : UserRequest<PostPreviewDto>
{
    public Guid PostId { get; set; }
}