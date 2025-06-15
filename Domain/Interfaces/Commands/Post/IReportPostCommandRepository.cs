using Domain.Entities.Posts;
using Domain.Enums.Common;

namespace Domain.Interfaces.Commands.Post
{
    public interface IReportPostCommandRepository
    {
        Task CreateReportPostAsync(ReportPost report);
        Task DeleteReportPostAsync(Guid reportPostId);
        Task<ReportPost> ReviewReport(Guid id, ReportStatusType status, string reviewerId);
    }
}
