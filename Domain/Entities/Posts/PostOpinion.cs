using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Common;

namespace Domain.Entities.Posts;

public class PostOpinion : Opinion
{
    public Guid PostId { get; set; }

    [ForeignKey("PostId")] public virtual Post Post { get; set; } = default!;
}