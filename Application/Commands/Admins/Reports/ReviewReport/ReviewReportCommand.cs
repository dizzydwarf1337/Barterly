using Application.Core.MediatR.Requests;
using Domain.Entities.Common;
using Domain.Enums.Common;

namespace Application.Commands.Admins.Reports.ReviewReport;

public class ReviewReportCommand : AdminRequest<Report>
{
    public Guid ReportId { get; set; }

    public ReportStatusType ReportStatus { get; set; }
}