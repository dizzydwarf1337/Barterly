using Application.DTOs.General;
using Domain.Enums.Common;

namespace Application.Interfaces
{
    public interface IReportService
    {
        Task SendReport(ReportDto reportDto);
        Task ChangeReportStatus(Guid reportId, ReportStatusType reportStatusType);
        Task DeleteReport(Guid reportId);
        Task<ReportDto> GetReportById(Guid reportId);
        Task<ICollection<ReportDto>> GetReportsByAuthorId(Guid authorId);
        Task<ICollection<ReportDto>> GetReportsByRepotedSubjectId(Guid repotedSubjectId);

    }
}
