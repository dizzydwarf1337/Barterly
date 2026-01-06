using Application.Core.MediatR.Requests;
using Application.DTOs.Posts;

namespace Application.Queries.Users.Posts.GetUsersPosts;

public class GetUsersPostsQuery : UserRequest<ICollection<PostPreviewDto>>
{
    public Guid UserId { get; set; }
}