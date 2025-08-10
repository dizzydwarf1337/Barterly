using Domain.Entities.Posts;
using Domain.Enums.Common;

namespace Domain.Interfaces.Commands.Post;

public interface IReportPostCommandRepository
{
    Task CreateReportPostAsync(ReportPost report, CancellationToken token);
    Task DeleteReportPostAsync(Guid reportPostId, CancellationToken token);
    Task<ReportPost> ReviewReport(Guid id, ReportStatusType status, Guid reviewerId, CancellationToken token);
}