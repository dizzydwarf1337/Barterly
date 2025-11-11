using FluentValidation;

namespace Application.Commands.Admins.Categories.EditCategory;

public class EditCategoryCommandValidator : AbstractValidator<EditCategoryCommand>
{
    public EditCategoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .Must(x => x != Guid.Empty).WithMessage("Id cannot be empty");
        RuleFor(x => x.NameEn)
            .NotEmpty().WithMessage("English name is required.")
            .MaximumLength(100).WithMessage("English name must not exceed 100 characters.");
        RuleFor(x => x.NamePl)
            .NotEmpty().WithMessage("Polish name is required.")
            .MaximumLength(100).WithMessage("Polish name must not exceed 100 characters.");
        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
    }
}