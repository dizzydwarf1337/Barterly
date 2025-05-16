using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Categories
{
    public class CategoryDto
    {
        [Required]
        public Guid Id { get; set; }
        
        [Required(ErrorMessage = "NamePL is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "NamePL must be between 2 and 50 characters long")]
        public string NamePL { get; set; }

        [Required(ErrorMessage = "NameEN is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "NameEN must be between 2 and 50 characters long")]
        public string NameEN { get; set; }
        [StringLength(200, ErrorMessage = "Description must be less than 200 characters long")]
        public string? Description { get; set; }
        public List<SubCategoryDto> SubCategories { get; set; }

    }
}
