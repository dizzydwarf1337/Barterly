using Application.Core.MediatR.Requests;

namespace Application.Queries.Admins.Categories.GetAllCategories;

public class GetAllCategoriesQuery : AdminRequest<IEnumerable<GetAllCategoriesQuery.Result>>
{
    public record Result(
        Guid Id,
        string NameEN,
        string NamePL,
        string Description,
        IEnumerable<SubCategory> SubCategories);

    public record SubCategory(Guid Id, string NameEN, string NamePL);
}