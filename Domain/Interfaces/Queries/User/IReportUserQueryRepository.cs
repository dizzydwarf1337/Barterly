using Domain.Entities.Users;
using Domain.Enums.Common;

namespace Domain.Interfaces.Queries.User;

public interface IReportUserQueryRepository
{
    Task<ReportUser> GetReportUserByIdAsync(Guid reportId, CancellationToken token);
    Task<ICollection<ReportUser>> GetReportUserByAuthorIdAsync(Guid authorId, CancellationToken token);

    Task<ICollection<ReportUser>> GetReportUserFiltredPaginated(int page, int pageSize, Guid? AuthorId,
        Guid? UserId, ReportStatusType? status, CancellationToken token);
}