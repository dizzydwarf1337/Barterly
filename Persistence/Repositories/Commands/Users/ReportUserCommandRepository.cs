using Domain.Entities.Users;
using Domain.Enums.Common;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Commands.User;
using Persistence.Database;

namespace Persistence.Repositories.Commands.Users;

public class ReportUserCommandRepository : BaseCommandRepository<BarterlyDbContext>, IReportUserCommandRepository
{
    public ReportUserCommandRepository(BarterlyDbContext context) : base(context)
    {
    }

    public async Task CreateReport(ReportUser report, CancellationToken token)
    {
        _ = await _context.Users.FindAsync(report.ReportedUserId) ?? throw new EntityNotFoundException("User");
        await _context.ReportUsers.AddAsync(report);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteReport(Guid reportId, CancellationToken token)
    {
        var report = await _context.ReportUsers.FindAsync(reportId) ??
                     throw new EntityNotFoundException("ReportUser");
        ;
        _context.ReportUsers.Remove(report);
        await _context.SaveChangesAsync();
    }

    public async Task<ReportUser> ReviewReport(Guid id, ReportStatusType status, Guid reviewerId,
        CancellationToken token)
    {
        var report = await _context.ReportUsers.FindAsync(id, token) ?? throw new EntityNotFoundException("Report");
        report.Status = status;
        report.ReviewedBy = reviewerId;
        report.ReviewedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(token);
        return report;
    }
}