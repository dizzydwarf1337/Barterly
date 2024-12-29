using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Log
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [MaxLength(500)]
        public string Message { get; set; }
        
        public string? StackTrace { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public Guid? UserId { get; set; }
       
        public Guid? PostId { get; set; }
    }
}
