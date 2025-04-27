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

        public async Task ChangeReportStatus(Guid id, ReportStatusType status)
        {
            var report = await _context.ReportUsers.FindAsync(id) ?? throw new EntityNotFoundException("ReportUser");
            report.Status = status;
            await _context.SaveChangesAsync();
        }

        public async Task CreateReport(ReportUser report)
        {
            await _context.ReportUsers.AddAsync(report);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteReport(Guid reportId)
        {
            var report = await _context.ReportUsers.FindAsync(reportId) ?? throw new EntityNotFoundException("ReportUser"); ;
            _context.ReportUsers.Remove(report);
            await _context.SaveChangesAsync();
        }
    }
}
