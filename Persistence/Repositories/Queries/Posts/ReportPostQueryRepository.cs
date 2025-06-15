using Domain.Entities.Posts;
using Domain.Entities.Users;
using Domain.Enums.Common;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Queries.Post;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories.Queries.Post
{
    public class ReportPostQueryRepository : BaseQueryRepository<BarterlyDbContext>, IReportPostQueryRepository
    {
        public ReportPostQueryRepository(BarterlyDbContext context) : base(context)
        {
        }
        public async Task<ReportPost> GetReportPostByIdAsync(Guid reportId)
        {
            return await _context.ReportPosts.FindAsync(reportId) ?? throw new EntityNotFoundException("Post report");
        }

        public async Task<List<ReportPost>> GetReportPostsFiltredPaginated(int page, int pageSize, Guid? authorId, Guid? postId, ReportStatusType? status)
        {
            var query = _context.ReportPosts.AsQueryable();
            if (authorId != null)
            {
                query = query.Where(x => x.AuthorId == authorId);
            }
            if (postId != null)
            {
                query = query.Where(x => x.ReportedPostId == postId);
            }
            if (status != null)
            {
                query = query.Where(x => x.Status == status);
            }

            return await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

        }
    }
}

