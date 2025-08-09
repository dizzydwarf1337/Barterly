using System.Text.RegularExpressions;
using FluentValidation;

namespace Application.Commands.Public.Accounts.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().WithName("Email");
        RuleFor(x => x.FirstName).NotEmpty().MinimumLength(2).MaximumLength(20).WithName("Imię");
        RuleFor(x => x.LastName).NotEmpty().MinimumLength(2).MaximumLength(50).WithName("Nazwisko");
        RuleFor(x => x.Password)
            .Must(password =>
                !string.IsNullOrEmpty(password) &&
                Regex.IsMatch(password, @"^(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).*$", RegexOptions.Compiled)
            )
            .WithMessage("Hasło musi zawierać co najmniej jedną cyfrę, wielką literę i znak specjalny");
    }
}