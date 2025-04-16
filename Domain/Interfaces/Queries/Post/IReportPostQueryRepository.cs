using Domain.Entities.Posts;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Queries.Post
{
    public interface IReportPostQueryRepository
    {
        Task<ReportPost> GetReportPostByIdAsync(Guid reportId);
        Task<ICollection<ReportPost>> GetAllPostReportsAsync();
        Task<ICollection<ReportPost>> GetReportPostsByTypeAsync(ReportStatusType type);
        Task<ICollection<ReportPost>> GetReportPostsByAuthorIdAsync(Guid authorId);
        Task<ICollection<ReportPost>> GetReportsPostByPostIdAsync(Guid postId);

    }
}
