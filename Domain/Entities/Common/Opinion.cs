using Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Common
{
    public abstract class Opinion
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid AuthorId { get; set; }

        [Required]
        [MaxLength(500)]
        public string Content { get; set; }

        public bool IsHidden { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastUpdatedAt { get; set; }

        [Range(1, 5)]
        public int? Rate { get; set; } = null;

        [ForeignKey("AuthorId")]
        public virtual User Author { get; set; }
    }
}