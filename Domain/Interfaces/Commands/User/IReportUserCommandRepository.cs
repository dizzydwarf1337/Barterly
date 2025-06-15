using Domain.Entities.Users;
using Domain.Enums.Common;

namespace Domain.Interfaces.Commands.User
{
    public interface IReportUserCommandRepository
    {
        Task CreateReport(ReportUser report);
        Task DeleteReport(Guid reportId);
        Task<ReportUser> ReviewReport(Guid id, ReportStatusType status,string reviewerId);
    }
}
