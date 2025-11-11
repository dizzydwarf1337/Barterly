using Domain.Enums.Posts;
using FluentValidation;

namespace Application.Commands.Admins.Posts.UpdatePostSettings;

public class UpdatePostSettingsCommandValidator : AbstractValidator<UpdatePostSettingsCommand>
{
    public UpdatePostSettingsCommandValidator()
    {
        RuleFor(x => x.RejectionMessage)
            .Must(x => x != null && x.Length > 20).When(x => x.PostStatusType == PostStatusType.Rejected).WithMessage("Rejection message is required when rejected");
    }
}