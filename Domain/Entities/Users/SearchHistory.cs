using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Categories;

namespace Domain.Entities.Users;

public class SearchHistory
{
    [Key] public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; }

    [Required] [MaxLength(100)] public required string SearchedText { get; set; }

    public string? SearchedCity { get; set; }

    public Guid? SearchedCategoryId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey("UserId")] public virtual User User { get; set; } = default!;
    public virtual Category? SearchedCategory { get; set; }
}