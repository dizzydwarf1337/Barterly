using Domain.Entities.Posts;
using Domain.Entities.Users;
using System.ComponentModel.DataAnnotations.Schema;

public class VisitedPost
{
    public Guid PostId { get; set; }
    public Guid UserId { get; set; }
    public DateTime LastVisitedAt { get; set; } = DateTime.UtcNow;
    public int VisitedCount { get; set; } = 1;

    [ForeignKey("PostId")]
    public virtual Post Post { get; set; }
    [ForeignKey("UserId")]
    public virtual User User { get; set; }
}
