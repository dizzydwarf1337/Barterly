using Application.Core.MediatR.Requests;
using Application.DTOs.Posts;

namespace Application.Queries.Public.Posts.GetPostImages;

public class GetPostImagesCommand : PublicRequest<PostImagesDto>
{
    public Guid PostId { get; set; }
}