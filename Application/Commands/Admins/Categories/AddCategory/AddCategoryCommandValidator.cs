using FluentValidation;

namespace Application.Commands.Admins.Categories.AddCategory;

public class AddCategoryCommandValidator : AbstractValidator<AddCategoryCommand>
{
    public AddCategoryCommandValidator()
    {
        RuleFor(x => x.NameEN)
            .NotEmpty().WithMessage("English name is required.")
            .MaximumLength(100).WithMessage("English name must not exceed 100 characters.");
        RuleFor(x => x.NamePL)
            .NotEmpty().WithMessage("Polish name is required.")
            .MaximumLength(100).WithMessage("Polish name must not exceed 100 characters.");
        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("English description must not exceed 500 characters.");
        RuleForEach(x => x.SubCategories).SetValidator(new SubCategoryValidator())
            .When(x=>x.SubCategories.Count > 0);
    }
    
}

public class SubCategoryValidator : AbstractValidator<AddCategoryCommand.SubCategory>
{
    public SubCategoryValidator()
    {
        RuleFor(x => x.NameEn).NotEmpty().WithMessage("English name is required.");
        RuleFor(x => x.NamePl).NotEmpty().WithMessage("Polish name is required.");
    }
}