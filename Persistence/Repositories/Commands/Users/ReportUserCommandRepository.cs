using Domain.Entities.Users;
using Domain.Enums;
using Domain.Interfaces.Commands.User;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.Commands.Users
{
    public class ReportUserCommandRepository : BaseCommandRepository<BarterlyDbContext>, IReportUserCommandRepository
    {
        public ReportUserCommandRepository(BarterlyDbContext context) : base(context)
        {
        }

        public async Task ChangeReportStatus(Guid id, ReportStatusType status)
        {
            var report = await _context.ReportUsers.FindAsync(id) ?? throw new Exception("ReportUser not found");
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
            var report = await _context.ReportUsers.FindAsync(reportId) ?? throw new Exception("ReportUser not found");
            _context.ReportUsers.Remove(report);
            await _context.SaveChangesAsync();
        }
    }
}
