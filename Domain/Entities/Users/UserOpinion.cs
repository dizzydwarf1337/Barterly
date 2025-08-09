using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Common;

namespace Domain.Entities.Users;

public class UserOpinion : Opinion
{
    public Guid UserId { get; set; }
    [ForeignKey("UserId")] public virtual User User { get; set; } = default!;
}