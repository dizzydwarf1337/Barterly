using Application.DTOs;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IReportService
    {
        Task SendReport(ReportDto reportDto);
        Task ChangeReportStatus(Guid reportId, ReportStatusType reportStatusType);
        Task DeleteReport(Guid reportId);
        Task<ICollection<ReportDto>> GetAllReports();
        Task<ICollection<ReportDto>> GetReportsByAuthorId(Guid authorId);
        Task<ICollection<ReportDto>> GetReportsByRepotedSubjectId(Guid repotedSubjectId);

    }
}
