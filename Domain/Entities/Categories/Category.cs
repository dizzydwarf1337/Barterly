using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Categories;

public class Category
{
    [Key] public Guid Id { get; set; } = Guid.NewGuid();

    [Required] [MaxLength(20)] public required string NamePL { get; set; }
    [Required] [MaxLength(20)] public required string NameEN { get; set; }

    [MaxLength(50)] public string? Description { get; set; }

    public virtual ICollection<SubCategory> SubCategories { get; set; } = [];
}