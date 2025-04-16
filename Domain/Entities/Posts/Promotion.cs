using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Posts
{
    public class Promotion
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public PostPromotionType Type = PostPromotionType.None;
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime EndDate { get; set; } = DateTime.MaxValue;
        public Guid PostId { get; set; }
        public virtual Post Post { get; set; }
    }
}
