using Application.Core.MediatR.Requests;
using Application.DTOs.Posts;

namespace Application.Queries.Users.Posts.GetFeed;

public class GetFeedQuery : UserRequest<ICollection<PostPreviewDto>>
{
    public int PageNumber { get; set; } = 10;
}