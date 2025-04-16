using Domain.Entities.Posts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Users
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
