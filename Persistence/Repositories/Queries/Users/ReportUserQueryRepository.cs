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

        public async Task<ReportUser> GetReportUserByIdAsync(Guid reportId)
        {
            return await _context.ReportUsers.FindAsync(reportId) ?? throw new EntityNotFoundException("User report");
        }

        public async Task<ICollection<ReportUser>> GetReportUserFiltredPaginated(int page, int pageSize, Guid? authorId, Guid? userId, ReportStatusType? status)
        {
            var query = _context.ReportUsers.AsQueryable();
            if(authorId != null)
            {
                query = query.Where(x => x.AuthorId == authorId);
            }
            if(userId != null)
            {
                query = query.Where(x => x.ReportedUserId == userId);
            }
            if(status != null)
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
