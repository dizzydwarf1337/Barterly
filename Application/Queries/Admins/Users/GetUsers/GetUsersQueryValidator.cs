using FluentValidation;

namespace Application.Queries.Admins.Users.GetUsers;

public class GetUsersQueryValidator : AbstractValidator<GetUsersQuery>
{
    public GetUsersQueryValidator()
    {
        RuleFor(x=>x.FilterBy).NotNull();
    }
}