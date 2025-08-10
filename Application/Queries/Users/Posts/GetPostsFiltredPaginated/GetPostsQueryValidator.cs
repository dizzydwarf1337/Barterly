using FluentValidation;

namespace Application.Queries.Users.Posts.GetPostsFiltredPaginated;

public class GetPostsQueryValidator : AbstractValidator<GetPostsQuery>
{
    public GetPostsQueryValidator()
    {
        RuleFor(x => x.FilterBy)
            .NotNull().WithMessage("FilterBy cannot be null.");
        RuleFor(x => x.FilterBy.PageSize)
            .GreaterThan(0).WithMessage("Page size must be greater than 0.");
        RuleFor(x => x.FilterBy.PageSize)
            .GreaterThan(0).WithMessage("Page size must be greater than 0.");
    }
}