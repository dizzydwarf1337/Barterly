using System.Text.RegularExpressions;
using FluentValidation;

namespace Application.Commands.Public.Auth.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().WithName("Email");
        RuleFor(x => x.Password)
            .Must(password =>
                !string.IsNullOrEmpty(password) &&
                Regex.IsMatch(password, @"^(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).*$", RegexOptions.Compiled)
            )
            .WithMessage("Hasło musi zawierać co najmniej jedną cyfrę, wielką literę i znak specjalny");
    }
}