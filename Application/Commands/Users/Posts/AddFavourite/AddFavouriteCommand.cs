using Application.Core.MediatR.Requests;
using MediatR;

namespace Application.Commands.Users.Posts.AddFavourite;

public class AddFavouriteCommand : UserRequest<Unit>
{
    public Guid Id { get; set; }
}