using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Posts
{
    public class PostImage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid PostId { get; set; }

        [Required]
        public required string ImageUrl { get; set; }

        [ForeignKey("PostId")]
        public virtual Post Post { get; set; } = default!;
    }
}
