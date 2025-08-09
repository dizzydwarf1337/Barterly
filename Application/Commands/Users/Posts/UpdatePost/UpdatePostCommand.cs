using Application.Core.MediatR.Requests;
using Application.DTOs.Posts;
using Application.Interfaces.CommandInterfaces;

namespace Application.Commands.Users.Posts.UpdatePost;

public class UpdatePostCommand : UserRequest<PostDto>, IHasOwner, IPostOwner
{
    public required EditPostDto Post { get; set; }

    public Guid OwnerId => Post.OwnerId;

    public Guid PostId => Post.Id;
}