using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Common;

public class MainAnnounsment
{
    [Key] public Guid Id { get; set; } = Guid.NewGuid();

    [MaxLength(50)] public required string Name { get; set; }

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}