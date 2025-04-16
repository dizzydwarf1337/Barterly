using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Users
{
    public class ReportUser : Report
    {
        public Guid ReportedUserId { get; set; }

        [ForeignKey("ReportedUserId")]
        public virtual User ReportedUser { get; set; }
    }
}
