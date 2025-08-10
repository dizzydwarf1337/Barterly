using FluentValidation;

namespace Application.Commands.Users.Posts.UpdatePost;

public class UpdatePostCommandValidator : AbstractValidator<UpdatePostCommand>
{
    public UpdatePostCommandValidator()
    {
        RuleFor(x => x.Post).NotNull().WithMessage("Post is required");

        When(x => x.Post != null, () =>
        {
            RuleFor(x => x.Post.Id)
                .NotEmpty().WithMessage("Id is required");

            RuleFor(x => x.Post.OwnerId)
                .NotEmpty().WithMessage("OwnerId is required");

            RuleFor(x => x.Post.SubCategoryId)
                .NotEmpty().WithMessage("SubCategoryId is required");

            RuleFor(x => x.Post.Title)
                .NotEmpty().WithMessage("Title is required")
                .Length(2, 50).WithMessage("Title must be between 2 and 50 characters long");

            RuleFor(x => x.Post.FullDescription)
                .NotEmpty().WithMessage("Full description is required")
                .Length(2, 10000).WithMessage("FullDescription must be between 2 and 10000 characters long");

            RuleFor(x => x.Post.ShortDescription)
                .NotEmpty().WithMessage("Short description is required")
                .Length(2, 200).WithMessage("ShortDescription must be between 2 and 200 characters long");

            RuleFor(x => x.Post.Price)
                .GreaterThanOrEqualTo(0).When(x => x.Post.Price.HasValue)
                .WithMessage("Price must be a positive number");

            RuleFor(x => x.Post.MinSalary)
                .GreaterThanOrEqualTo(0).When(x => x.Post.MinSalary.HasValue)
                .WithMessage("MinSalary must be a positive number");

            RuleFor(x => x.Post.MaxSalary)
                .GreaterThanOrEqualTo(0).When(x => x.Post.MaxSalary.HasValue)
                .WithMessage("MaxSalary must be a positive number");

            RuleFor(x => x.Post.MaxSalary)
                .GreaterThanOrEqualTo(x => x.Post.MinSalary ?? 0)
                .When(x => x.Post.MinSalary.HasValue && x.Post.MaxSalary.HasValue)
                .WithMessage("MaxSalary must be greater than or equal to MinSalary");

            RuleFor(x => x.Post.NumberOfRooms)
                .GreaterThan(0).When(x => x.Post.NumberOfRooms.HasValue)
                .WithMessage("Number of rooms must be a positive number");

            RuleFor(x => x.Post.Area)
                .GreaterThan(0).When(x => x.Post.Area.HasValue)
                .WithMessage("Area must be a positive number");

            RuleFor(x => x.Post.Floor)
                .GreaterThanOrEqualTo(0).When(x => x.Post.Floor.HasValue)
                .WithMessage("Floor must be 0 or more");
        });
    }
}