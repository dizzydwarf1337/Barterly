using FluentValidation;

namespace Application.Commands.Public.Auth.LoginWithGoogle;

public class LoginWithGoogleCommandValidator : AbstractValidator<LoginWithGoogleCommand>
{
    public LoginWithGoogleCommandValidator()
    {
        RuleFor(x => x.token).NotEmpty().MinimumLength(10).WithMessage("Google token must be not null");
    }
}