using Application.Core.MediatR.Requests;
using Domain.Entities.Common;
using Domain.Enums.Common;

namespace Application.Commands.Moderators.Reports.ReviewReport;

public class ReviewReportCommand : ModeratorRequest<Report>
{
    public Guid ReportId { get; set; }

    public ReportStatusType ReportStatus { get; set; }
}