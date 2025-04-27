using Domain.Entities.Posts;
using Domain.Enums.Common;
using Domain.Exceptions.BusinessExceptions;
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
            var report = await _context.ReportPosts.FindAsync(id) ?? throw new EntityNotFoundException("PostReport");
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
            var report = await _context.ReportPosts.FindAsync(reportPostId) ?? throw new EntityNotFoundException("PostReport");
            _context.ReportPosts.Remove(report);
            await _context.SaveChangesAsync();
        }
    }
}
