using Domain.Entities.Users;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Queries.User
{
    public interface IReportUserQueryRepository
    {
        Task<ReportUser> GetReportUserByIdAsync(Guid reportId);
        Task<ICollection<ReportUser>> GetAllUsersReportsAsync();
        Task<ICollection<ReportUser>> GetReportUsersByTypeAsync(ReportStatusType type);
        Task<ICollection<ReportUser>> GetReportUsersByAuthorIdAsync(Guid authorId);
        Task<ICollection<ReportUser>> GetReportsUserByUserIdAsync(Guid userId);

    }
}
