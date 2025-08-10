using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Posts;

public class PostOpinionDto
{
    [Required] public required string Id { get; set; }
    [Required] public required string AuthorId { get; set; }

    [Required]
    [StringLength(300, MinimumLength = 2, ErrorMessage = "Content length must be between 2 and 300")]
    public required string Content { get; set; }

    public bool IsHidden { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? LastUpdatedAt { get; set; }

    public int? Rate { get; set; } = null;
}