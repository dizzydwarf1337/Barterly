using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.General;

public class IdDto
{
    [Required(ErrorMessage = "Id is required")]
    public required string Id { get; set; }
}