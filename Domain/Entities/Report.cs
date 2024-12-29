using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public abstract class Report
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(200)]
        public string Message { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public ReportStatusType Status { get; set; } = ReportStatusType.Submitted;
        
        public Guid AuthorId { get; set; }
        
        [ForeignKey("AuthorId")]
        public virtual User Author { get; set; }
    }
}
