using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class MainAnnounsment
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [MaxLength(50)]
        public string Name { get; set; }
       
        public string? Description { get; set; }
       
        public string? ImageUrl { get; set; }
      
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
