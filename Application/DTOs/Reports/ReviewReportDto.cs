using Domain.Enums.Common;

namespace Application.DTOs.Reports;

public class ReviewReportDto
{
    public required string ReportId { get; set; }
    public required ReportStatusType ReportStatus { get; set; }
}