using FluentValidation;

namespace Application.Commands.Admins.Opinions.DeleteOpinion;

public class DeleteOpinionCommandValidator : AbstractValidator<DeleteOpinionCommand>
{
    public DeleteOpinionCommandValidator()
    {
        RuleFor(x => x.OpinionId)
            .NotEmpty().WithMessage("Opinion ID is required.")
            .Must(id => id != Guid.Empty).WithMessage("Opinion ID must be a valid GUID.");
    }
}