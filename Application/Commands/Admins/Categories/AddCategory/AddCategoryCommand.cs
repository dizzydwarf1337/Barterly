using Application.Core.MediatR.Requests;
using Application.DTOs.Categories;
using MediatR;

namespace Application.Commands.Admins.Categories.AddCategory;

public class AddCategoryCommand : AdminRequest<Unit>
{
    public required string NamePL { get; set; }
    public required string NameEN { get; set; }
    public string? Description { get; set; }
    public List<SubCategory> SubCategories { get; set; }

    public record SubCategory(string NameEn, string NamePl);
}