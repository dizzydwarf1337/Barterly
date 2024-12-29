using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ReportPost : Report
    {
        public Guid ReportedPostId { get; set; }
        
        [ForeignKey("PostId")]
        public virtual Post ReportedPost { get; set; }
    }
}
