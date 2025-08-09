using Application.Core.MediatR.Requests;

namespace Application.Queries.Public.Categories.GetAllCategories;

public class GetAllCategoriesQuery : PublicRequest<IEnumerable<GetAllCategoriesQuery.Result>>
{
    public record Result(
        Guid Id,
        string NameEN,
        string NamePL,
        string Description,
        IEnumerable<SubCategory> SubCategories);

    public record SubCategory(Guid Id, string NameEN, string NamePL);
}