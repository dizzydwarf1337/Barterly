using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Reports
{
    public class CreateReportDto
    {
        public required string AuthorId { get; set; }
        public required string SubjectId { get; set; }
        public required string Message { get; set; }
        public required string ReportType { get; set; }
    }
}
