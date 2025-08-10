using Application.Core.MediatR.Requests;
using Application.DTOs.Posts;
using Application.Interfaces.CommandInterfaces;
using MediatR;

namespace Application.Commands.Users.Posts.UpdatePostImages;

public class UpdatePostImagesCommand : UserRequest<Unit>, IHasOwner, IPostOwner
{
    public required ImagesDto ImagesDto { get; set; }

    public Guid OwnerId => AuthorizeData.UserId;

    public Guid PostId => ImagesDto.PostId;
}