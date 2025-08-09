using Domain.Entities.Posts;
using Domain.Enums.Common;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Commands.Post;
using Persistence.Database;

namespace Persistence.Repositories.Commands.Post;

public class ReportPostCommandRepository : BaseCommandRepository<BarterlyDbContext>, IReportPostCommandRepository
{
    public ReportPostCommandRepository(BarterlyDbContext context) : base(context)
    {
    }

    public async Task CreateReportPostAsync(ReportPost report, CancellationToken token)
    {
        _ = await _context.Posts.FindAsync(report.ReportedPostId, token) ??
            throw new EntityNotFoundException("Post");
        await _context.ReportPosts.AddAsync(report, token);
        await _context.SaveChangesAsync(token);
    }

    public async Task DeleteReportPostAsync(Guid reportPostId, CancellationToken token)
    {
        var report = await _context.ReportPosts.FindAsync(reportPostId, token) ??
                     throw new EntityNotFoundException("PostReport");
        _context.ReportPosts.Remove(report);
        await _context.SaveChangesAsync(token);
    }

    public async Task<ReportPost> ReviewReport(Guid id, ReportStatusType status, Guid reviewerId,
        CancellationToken token)
    {
        var report = await _context.ReportPosts.FindAsync(id, token) ?? throw new EntityNotFoundException("Report");
        report.Status = status;
        report.ReviewedBy = reviewerId;
        report.ReviewedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(token);
        return report;
    }
}