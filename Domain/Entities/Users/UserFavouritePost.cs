using Domain.Entities.Posts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Users
{
    public class UserFavouritePost
    {
        [Column(Order = 0)]
        public Guid PostId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual Post Post { get; set; }
        [Key]
        [Column(Order = 1)]
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
    }
}
