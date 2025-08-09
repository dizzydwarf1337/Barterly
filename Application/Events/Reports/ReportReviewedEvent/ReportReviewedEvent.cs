using Domain.Entities.Common;
using MediatR;

namespace Application.Events.Reports.ReportReviewedEvent;

public class ReportReviewedEvent : INotification
{
    public required Report Report { get; set; }
}