using FluentValidation;

namespace Application.Queries.Public.Posts.GetFeed;

public class GetFeedQueryValidator : AbstractValidator<GetFeedQuery>
{
    public GetFeedQueryValidator()
    {
        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("Page size must be greater than 0.");
        RuleFor(x => x.PageNumber)
            .GreaterThan(0).WithMessage("Page number must be greater than 0.");
    }
}