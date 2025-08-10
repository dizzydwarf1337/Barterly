using FluentValidation;

namespace Application.Commands.Users.Opinions.EditOpinion;

public class EditOpinionCommandValidator : AbstractValidator<EditOpinionCommand>
{
    public EditOpinionCommandValidator()
    {
        RuleFor(x => x.Message).MaximumLength(500).WithName("Opinia");
        RuleFor(x => x.Rate).Must(x => x <= 5 && x >= 1).NotNull();
        RuleFor(x => x.OpinionId).NotEmpty();
        RuleFor(x => x.OwnerId).NotEmpty();
        RuleFor(x => x.OpinionType).NotEmpty();
    }
}