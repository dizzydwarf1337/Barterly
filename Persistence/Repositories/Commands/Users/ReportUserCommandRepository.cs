using Domain.Entities.Users;
using Domain.Enums.Common;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Commands.User;
using Persistence.Database;

namespace Persistence.Repositories.Commands.Users
{
    public class ReportUserCommandRepository : BaseCommandRepository<BarterlyDbContext>, IReportUserCommandRepository
    {
        public ReportUserCommandRepository(BarterlyDbContext context) : base(context)
        {
        }

        public async Task CreateReport(ReportUser report)
        {
            _ = await _context.Users.FindAsync(report.ReportedUserId) ?? throw new EntityNotFoundException("User");
            await _context.ReportUsers.AddAsync(report); 
            await _context.SaveChangesAsync();
        }

        public async Task DeleteReport(Guid reportId)
        {
            var report = await _context.ReportUsers.FindAsync(reportId) ?? throw new EntityNotFoundException("ReportUser"); ;
            _context.ReportUsers.Remove(report);
            await _context.SaveChangesAsync();
        }

        public async Task<ReportUser> ReviewReport(Guid id, ReportStatusType status, string reviewerId)
        {
            var report = await _context.ReportUsers.FindAsync(id) ?? throw new EntityNotFoundException("Report");
            report.Status = status;
            report.ReviewedAt = DateTime.Now;
            report.ReviewedBy = reviewerId;
            await _context.SaveChangesAsync();
            return report;
        }

    }
}
