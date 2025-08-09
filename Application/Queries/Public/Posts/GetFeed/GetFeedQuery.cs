using Application.Core.MediatR.Requests;
using Application.DTOs.Posts;

namespace Application.Queries.Public.Posts.GetFeed;

public class GetFeedQuery : PublicRequest<ICollection<PostPreviewDto>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}