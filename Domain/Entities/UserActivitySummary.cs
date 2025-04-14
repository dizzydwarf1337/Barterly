using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserActivitySummary
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
       
        public int TotalPostsVisited { get; set; }

        public string MostViewedCategories { get; set; } = String.Empty;
        
        public string MostViewedCities { get; set; } = String.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = null;

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
