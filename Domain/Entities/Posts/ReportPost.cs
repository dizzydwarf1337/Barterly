using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Common;

namespace Domain.Entities.Posts;

public class ReportPost : Report
{
    public Guid ReportedPostId { get; set; }

    [ForeignKey("ReportedPostId")] public virtual Post ReportedPost { get; set; } = default!;
}