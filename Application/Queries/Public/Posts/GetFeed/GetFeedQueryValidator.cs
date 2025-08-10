using FluentValidation;

namespace Application.Queries.Public.Posts.GetFeed;

public class GetFeedQueryValidator : AbstractValidator<GetFeedQuery>
{
    public GetFeedQueryValidator()
    {
        RuleFor(x=>x.FilterBy).NotNull().NotEmpty();
    }
}