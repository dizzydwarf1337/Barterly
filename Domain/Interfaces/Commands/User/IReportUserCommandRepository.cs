using Domain.Entities.Users;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Commands.User
{
    public interface IReportUserCommandRepository
    {
        Task CreateReport(ReportUser report);
        Task DeleteReport(Guid reportId);
        Task ChangeReportStatus(Guid id, ReportStatusType status);
    }
}
