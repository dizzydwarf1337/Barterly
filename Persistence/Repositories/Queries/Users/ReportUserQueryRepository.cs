using Domain.Entities.Users;
using Domain.Enums.Common;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Queries.User;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories.Queries.Users
{
    public class ReportUserQueryRepository : BaseQueryRepository<BarterlyDbContext>, IReportUserQueryRepository
    {
        public ReportUserQueryRepository(BarterlyDbContext context) : base(context)
        {
        }

        public async Task<ICollection<ReportUser>> GetAllUsersReportsAsync()
        {
            return await _context.ReportUsers.ToListAsync();
        }

        public async Task<ICollection<ReportUser>> GetReportsUserByUserIdAsync(Guid userId)
        {
            return await _context.ReportUsers.Where(x => x.ReportedUserId == userId).ToListAsync();
        }

        public async Task<ReportUser> GetReportUserByIdAsync(Guid reportId)
        {
            return await _context.ReportUsers.FindAsync(reportId) ?? throw new EntityNotFoundException("User report");
        }

        public async Task<ICollection<ReportUser>> GetReportUsersByAuthorIdAsync(Guid authorId)
        {
            return await _context.ReportUsers.Where(x => x.AuthorId == authorId).ToListAsync();
        }

        public async Task<ICollection<ReportUser>> GetReportUsersByTypeAsync(ReportStatusType type)
        {
            return await _context.ReportUsers.Where(x => x.Status == type).ToListAsync();
        }
    }
}
