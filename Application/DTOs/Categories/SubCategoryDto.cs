using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Categories
{
    public class SubCategoryDto
    {
        [Required]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "TitlePL is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "TitlePL must be between 2 and 50 characters long")]
        public string TitlePL { get; set; }
        [Required(ErrorMessage = "TitleEN is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "TitleEN must be between 2 and 50 characters long")]
        public string TitleEN { get; set; }
        [Required(ErrorMessage = "CategoryId is required")]
        public Guid CategoryId { get; set; }
    }
}
