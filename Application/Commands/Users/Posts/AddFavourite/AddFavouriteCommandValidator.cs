using FluentValidation;

namespace Application.Commands.Users.Posts.AddFavourite;

public class AddFavouriteCommandValidator : AbstractValidator<AddFavouriteCommand>
{
    public AddFavouriteCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}