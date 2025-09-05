using Application.Core.MediatR.Requests;
using Application.DTOs.Categories;
using Domain.Entities.Categories;

namespace Application.Commands.Admins.Categories.EditCategory;

public class EditCategoryCommand : AdminRequest<Category>
{
    public Guid Id { get; set; }
    public required string NamePl { get; set; }
    public required string NameEn { get; set; }
    public string? Description { get; set; }
    public List<SubCategoryDto> SubCategories { get; set; } = default!;
}