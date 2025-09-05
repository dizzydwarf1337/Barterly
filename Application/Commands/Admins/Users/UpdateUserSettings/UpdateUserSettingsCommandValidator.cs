using FluentValidation;

namespace Application.Commands.Admins.Users.UpdateUserSettings;

public class UpdateUserSettingsCommandValidator : AbstractValidator<UpdateUserSettingsCommand>
{
    public UpdateUserSettingsCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}