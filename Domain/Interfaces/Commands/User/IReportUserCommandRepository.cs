using Domain.Entities.Users;
using Domain.Enums.Common;

namespace Domain.Interfaces.Commands.User;

public interface IReportUserCommandRepository
{
    Task CreateReport(ReportUser report, CancellationToken token);
    Task DeleteReport(Guid reportId, CancellationToken token);
    Task<ReportUser> ReviewReport(Guid id, ReportStatusType status, Guid reviewerId, CancellationToken token);
}