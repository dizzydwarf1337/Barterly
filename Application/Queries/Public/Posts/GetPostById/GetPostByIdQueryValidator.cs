using FluentValidation;

namespace Application.Queries.Public.Posts.GetPostById;

public class GetPostByIdQueryValidator : AbstractValidator<GetPostByIdQuery>
{
    public GetPostByIdQueryValidator()
    {
        RuleFor(x => x.PostId)
            .NotEmpty().WithMessage("PostId cannot be empty.")
            .NotNull().WithMessage("PostId cannot be null.");
    }
}