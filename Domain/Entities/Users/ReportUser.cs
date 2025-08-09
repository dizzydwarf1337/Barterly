using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Common;

namespace Domain.Entities.Users;

public class ReportUser : Report
{
    public Guid ReportedUserId { get; set; }

    [ForeignKey("ReportedUserId")] public virtual User ReportedUser { get; set; } = default!;
}