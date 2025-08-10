using Application.Core.MediatR.Requests;
using MediatR;

namespace Application.Commands.Admins.Reports.DeleteReport;

public class DeleteReportCommand : AdminRequest<Unit>
{
    public required Guid ReportId { get; set; }
}