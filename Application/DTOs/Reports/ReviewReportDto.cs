using Domain.Enums.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Reports
{
    public class ReviewReportDto
    {
        public required string ReportId { get; set; }
        public required ReportStatusType ReportStatus { get; set; }
    }
}
