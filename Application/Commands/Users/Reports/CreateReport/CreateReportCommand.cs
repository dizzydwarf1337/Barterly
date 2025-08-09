using Application.Core.MediatR.Requests;
using Application.Interfaces.CommandInterfaces;
using MediatR;

namespace Application.Commands.Users.Reports.CreateReport;

public class CreateReportCommand : UserRequest<Unit>, IHasOwner, IMessageContainer
{
    public required Guid SubjectId { get; set; }
    public required string ReportType { get; set; }
    public required Guid OwnerId { get; set; }
    public required string Message { get; set; }
}