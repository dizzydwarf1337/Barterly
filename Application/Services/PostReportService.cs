using Application.DTOs;
using Application.ServiceInterfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PostReportService : IReportService
    {
        public Task ChangeReportStatus(Guid reportId, ReportStatusType reportStatusType)
        {
            throw new NotImplementedException();
        }

        public Task DeleteReport(Guid reportId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ReportDto>> GetAllReports()
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<ReportDto>> GetReportsByAuthorId(Guid authorId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<ReportDto>> GetReportsByRepotedSubjectId(Guid repotedSubjectId)
        {
            throw new NotImplementedException();
        }

        public Task SendReport(ReportDto reportDto)
        {
            throw new NotImplementedException();
        }
    }
}
