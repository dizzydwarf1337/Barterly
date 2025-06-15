using Domain.Entities.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Reports.Events.ReportReviewdEvent
{
    public class ReportReviewedEvent : INotification
    {
        public required Report Report { get; set; }
    }
}
