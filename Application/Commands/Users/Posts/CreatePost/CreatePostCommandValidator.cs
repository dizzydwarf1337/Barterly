using FluentValidation;

namespace Application.Commands.Users.Posts.CreatePost;

public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
{
    public CreatePostCommandValidator()
    {
        RuleFor(x => x.SubCategoryId)
            .NotEmpty().WithMessage("SubCategoryId is required");

        RuleFor(x => x.OwnerId)
            .NotEmpty().WithMessage("OwnerId is required");

        RuleFor(x => x.PostType)
            .NotEmpty().WithMessage("PostType is required");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .Length(2, 50).WithMessage("Title must be between 2 and 50 characters long");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("City is required");

        RuleFor(x => x.FullDescription)
            .NotEmpty().WithMessage("Full description is required")
            .Length(2, 10000).WithMessage("FullDescription must be between 2 and 10000 characters long");

        RuleFor(x => x.ShortDescription)
            .NotEmpty().WithMessage("Short description is required")
            .Length(2, 200).WithMessage("ShortDescription must be between 2 and 200 characters long");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).When(x => x.Price.HasValue)
            .WithMessage("Price must be a positive number");

        RuleFor(x => x.MinSalary)
            .GreaterThanOrEqualTo(0).When(x => x.MinSalary.HasValue)
            .WithMessage("MinSalary must be a positive number");

        RuleFor(x => x.MaxSalary)
            .GreaterThanOrEqualTo(0).When(x => x.MaxSalary.HasValue)
            .WithMessage("MaxSalary must be a positive number");

        RuleFor(x => x.MaxSalary)
            .GreaterThanOrEqualTo(x => x.MinSalary ?? 0).When(x => x.MinSalary.HasValue && x.MaxSalary.HasValue)
            .WithMessage("MaxSalary must be greater than or equal to MinSalary");

        RuleFor(x => x.Area)
            .GreaterThan(0).When(x => x.Area.HasValue)
            .WithMessage("Area must be a positive number");

        RuleFor(x => x.NumberOfRooms)
            .GreaterThan(0).When(x => x.NumberOfRooms.HasValue)
            .WithMessage("Number of rooms must be a positive number");

        RuleFor(x => x.Floor)
            .GreaterThanOrEqualTo(0).When(x => x.Floor.HasValue)
            .WithMessage("Floor must be 0 or more");

        RuleFor(x => x.MainImage)
            .Must(file => file == null || file.Length > 0)
            .WithMessage("Main image must not be empty");

        RuleForEach(x => x.SecondaryImages)
            .Must(file => file == null || file.Length > 0)
            .WithMessage("Secondary image must not be empty");
    }
}