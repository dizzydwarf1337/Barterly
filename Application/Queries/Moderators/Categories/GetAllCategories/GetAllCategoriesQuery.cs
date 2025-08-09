using Application.Core.MediatR.Requests;

namespace Application.Queries.Moderators.Categories.GetAllCategories;

public class GetAllCategoriesQuery : ModeratorRequest<IEnumerable<GetAllCategoriesQuery.Result>>
{
    public record Result(
        Guid Id,
        string NameEN,
        string NamePL,
        string Description,
        IEnumerable<SubCategory> SubCategories);

    public record SubCategory(Guid Id, string NameEN, string NamePL);
}