using Domain.Entities.Users;
using Domain.Enums.Common;

namespace Domain.Interfaces.Queries.User
{
    public interface IReportUserQueryRepository
    {
        Task<ReportUser> GetReportUserByIdAsync(Guid reportId);
        Task<ICollection<ReportUser>> GetReportUserFiltredPaginated(int page, int pageSize, Guid? AuthorId, Guid? UserId, ReportStatusType? status);

    }
}
