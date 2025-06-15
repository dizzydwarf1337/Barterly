using Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Posts
{
    public class PostOpinion : Opinion
    {
        public Guid PostId { get; set; }

        [ForeignKey("PostId")]
        public virtual Post Post { get; set; } = default!;
    }
}
