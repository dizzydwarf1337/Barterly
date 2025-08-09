using FluentValidation;

namespace Application.Queries.Admins.Posts.GetPostFiltredPaginated;

public class GetPostsQueryValidator : AbstractValidator<GetPostsQuery>
{
    public GetPostsQueryValidator()
    {
        RuleFor(x => x.FilterBy)
            .NotNull().WithMessage("FilterBy cannot be null.");
        RuleFor(x => x.FilterBy.PageSize)
            .GreaterThan(0).WithMessage("Page size must be greater than 0.");

        RuleFor(x => x.FilterBy.PageNumber)
            .GreaterThan(0).WithMessage("Page number must be greater than 0.");
    }
}