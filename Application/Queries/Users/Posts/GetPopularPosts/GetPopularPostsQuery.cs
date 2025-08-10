using Application.Core.MediatR.Requests;
using Application.DTOs.Posts;

namespace Application.Queries.Users.Posts.GetPopularPosts;

public class GetPopularPostsQuery : UserRequest<ICollection<PostPreviewDto>>
{
    public int Count { get; set; }
}