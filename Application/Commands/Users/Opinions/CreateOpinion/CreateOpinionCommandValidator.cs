using FluentValidation;

namespace Application.Commands.Users.Opinions.CreateOpinion;

public class CreateOpinionCommandValidator : AbstractValidator<CreateOpinionCommand>
{
    public CreateOpinionCommandValidator()
    {
        RuleFor(x => x.Message).MaximumLength(500).WithName("Opinia");
        RuleFor(x => x.Rate).Must(x => x <= 5 && x >= 1).NotNull();
        RuleFor(x => x.SubjectId).NotEmpty();
        RuleFor(x => x.OwnerId).NotEmpty();
        RuleFor(x => x.OpinionType).NotEmpty();
    }
}