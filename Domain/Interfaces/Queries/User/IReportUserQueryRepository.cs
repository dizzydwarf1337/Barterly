using Domain.Entities.Users;
using Domain.Enums;

namespace Domain.Interfaces.Queries.User
{
    public interface IReportUserQueryRepository
    {
        Task<ReportUser> GetReportUserByIdAsync(Guid reportId);
        Task<ICollection<ReportUser>> GetAllUsersReportsAsync();
        Task<ICollection<ReportUser>> GetReportUsersByTypeAsync(ReportStatusType type);
        Task<ICollection<ReportUser>> GetReportUsersByAuthorIdAsync(Guid authorId);
        Task<ICollection<ReportUser>> GetReportsUserByUserIdAsync(Guid userId);

    }
}
