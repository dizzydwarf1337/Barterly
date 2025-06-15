using Domain.Entities.Posts;
using Domain.Enums.Common;

namespace Domain.Interfaces.Queries.Post
{
    public interface IReportPostQueryRepository
    {
        Task<ReportPost> GetReportPostByIdAsync(Guid reportId);
        Task<List<ReportPost>> GetReportPostsFiltredPaginated(int pageSize, int page, Guid? AuthorId, Guid? postId, ReportStatusType? status);
    }
}
