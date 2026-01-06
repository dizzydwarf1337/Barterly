using FluentValidation;

namespace Application.Commands.Public.Accounts.ResendEmailConfirm;

public class ResendEmailConfirmValidator : AbstractValidator<ResendEmailConfirmCommand>
{
    public ResendEmailConfirmValidator()
    {
    }
}