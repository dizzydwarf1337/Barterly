using Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Posts
{
    public class ReportPost : Report
    {
        public Guid ReportedPostId { get; set; }

        [ForeignKey("PostId")]
        public virtual Post ReportedPost { get; set; }
    }
}
