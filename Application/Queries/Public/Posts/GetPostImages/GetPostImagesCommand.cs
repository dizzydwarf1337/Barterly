using Application.Core.MediatR.Requests;
using Application.DTOs.Posts;

namespace Application.Queries.Public.Posts.GetPostImages;

public class GetPostImagesCommand : PublicRequest<ICollection<PostImageDto>>
{
    public Guid PostId { get; set; }
}