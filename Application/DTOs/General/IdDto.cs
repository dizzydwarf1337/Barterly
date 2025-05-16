using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.General
{
    public class IdDto
    {
        [Required(ErrorMessage = "Id is required")]
        public Guid Id { get; set; }
    }
}
