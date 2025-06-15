using Domain.Entities.Users;
using Domain.Enums.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Common
{
    public abstract class Report
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(200)]
        public required string Message { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ReportStatusType Status { get; set; } = ReportStatusType.Submitted;

        public string? ReviewedBy { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public Guid AuthorId { get; set; }

        [ForeignKey("AuthorId")]
        public virtual User Author { get; set; } = default!;
    }
}
