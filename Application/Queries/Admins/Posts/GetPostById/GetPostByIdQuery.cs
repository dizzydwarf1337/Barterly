using Application.Core.MediatR.Requests;
using Application.DTOs.Posts;

namespace Application.Queries.Admins.Posts.GetPostById;

public class GetPostByIdQuery : AdminRequest<GetPostByIdQuery.Result>
{
    public required Guid PostId { get; set; }

    public record Result(PostDto Post, PostSettingsDto Settings, PostOpinionDto[] Opinions, OwnerData Owner);

    public record OwnerData(Guid OwnerId, string FirstName, string LastName);
}