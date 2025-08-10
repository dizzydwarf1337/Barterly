using Application.Core.MediatR.Requests;
using MediatR;

namespace Application.Commands.Moderators.Reports.DeleteReport;

public class DeleteReportCommand : ModeratorRequest<Unit>
{
    public required Guid ReportId { get; set; }
}