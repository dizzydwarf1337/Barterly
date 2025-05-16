using Domain.Enums.Common;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.General
{
    public class ReportDto
    {
        [Required]
        public Guid Id;
        [Required(ErrorMessage = "Message is required")]
        [StringLength(500, MinimumLength = 2, ErrorMessage = "Message must be between 2 and 500 characters long")]
        public string Message = "";
        public DateTime CreatedAt = DateTime.UtcNow;
        public ReportStatusType Status = ReportStatusType.Submitted;
        [Required(ErrorMessage = "AuthorId is required")]
        public Guid AuthorId;
        [Required(ErrorMessage = "RepotedSubjectId is required")]
        public Guid RepotedSubjectId;
    }
}
