using Domain.Entities.Posts;
using Domain.Enums;

namespace Domain.Interfaces.Commands.Post
{
    public interface IReportPostCommandRepository
    {
        Task CreateReportPostAsync(ReportPost report);
        Task DeleteReportPostAsync(Guid reportPostId);
        Task ChangeReportPostStatusAsync(Guid id, ReportStatusType status);
    }
}
