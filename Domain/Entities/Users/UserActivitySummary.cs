using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Users
{
    public class UserActivitySummary
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }

        public int TotalPostsVisited { get; set; }

        public string MostViewedCategories { get; set; } = string.Empty;

        public string MostViewedCities { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = null;

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = default!;
    }
}
