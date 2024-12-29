using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class VisitedPost
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid PostId { get; set; }

        public Guid UserId { get; set; }

        public DateTime LastVisitedAt { get; set; } = DateTime.UtcNow;

        public int VisitedCount { get; set; } = 1;

        [ForeignKey("PostId")]
        public virtual Post Post { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
