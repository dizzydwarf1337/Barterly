using Domain.Entities.Posts;
using Domain.Enums;
using Domain.Interfaces.Commands.Post;
using Persistence.Database;

namespace Persistence.Repositories.Commands.Post
{
    public class ReportPostCommandRepository : BaseCommandRepository<BarterlyDbContext>, IReportPostCommandRepository
    {
        public ReportPostCommandRepository(BarterlyDbContext context) : base(context)
        {
        }

        public async Task ChangeReportPostStatusAsync(Guid id, ReportStatusType status)
        {
            var report = await _context.ReportPosts.FindAsync(id) ?? throw new Exception("PostRepost not found");
            report.Status = status;
            await _context.SaveChangesAsync();
        }

        public async Task CreateReportPostAsync(ReportPost report)
        {
            await _context.ReportPosts.AddAsync(report);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteReportPostAsync(Guid reportPostId)
        {
            var report = await _context.ReportPosts.FindAsync(reportPostId) ?? throw new Exception("PostRepost not found");
            _context.ReportPosts.Remove(report);
            await _context.SaveChangesAsync();
        }
    }
}
