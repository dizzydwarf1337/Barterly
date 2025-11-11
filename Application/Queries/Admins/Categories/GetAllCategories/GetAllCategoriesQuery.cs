using Application.Core.MediatR.Requests;

namespace Application.Queries.Admins.Categories.GetAllCategories;

public class GetAllCategoriesQuery : AdminRequest<GetAllCategoriesQuery.Result>
{
    public FilterBy? FilterBy { get; set; }
    public SortBy? SortBy { get; set; }

    public record Result(
        IEnumerable<CategoryDto> Categories,
        int TotalCount,
        int TotalPages,
        int CurrentPage,
        int PageSize);

    public record CategoryDto(
        Guid Id,
        string NameEN,
        string NamePL,
        string Description,
        int SubCategoriesCount,
        IEnumerable<SubCategoryDto> SubCategories);

    public record SubCategoryDto(
        Guid Id, 
        string NameEN, 
        string NamePL);
}

public class FilterBy
{
    public string? Search { get; set; }
        
    public bool? HasSubCategories { get; set; }
        
    public int PageNumber { get; set; } = 1;
        
    public int PageSize { get; set; } = 10;
}

public class SortBy
{
    public string? SortField { get; set; } = "nameEn ";
        
    public bool IsDescending { get; set; } = true;
}