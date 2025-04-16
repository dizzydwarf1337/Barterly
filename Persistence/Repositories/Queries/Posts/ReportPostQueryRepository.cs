using Domain.Entities.Posts;
using Domain.Enums;
using Domain.Interfaces.Queries.Post;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.Queries.Post
{
    public class ReportPostQueryRepository : BaseQueryRepository<BarterlyDbContext>, IReportPostQueryRepository
    {
        public ReportPostQueryRepository(BarterlyDbContext context) : base(context)
        {
        }

        public async Task<ICollection<ReportPost>> GetAllPostReportsAsync()
        {
            return await _context.ReportPosts.ToListAsync();
        }

        public async Task<ReportPost> GetReportPostByIdAsync(Guid reportId)
        {
            return await _context.ReportPosts.FindAsync(reportId) ?? throw new Exception("Post Report not found");
        }

        public async Task<ICollection<ReportPost>> GetReportPostsByAuthorIdAsync(Guid authorId)
        {
            return await _context.ReportPosts.Where(x => x.AuthorId == authorId).ToListAsync();
        }

        public async Task<ICollection<ReportPost>> GetReportPostsByTypeAsync(ReportStatusType type)
        {
            return await _context.ReportPosts.Where(x => x.Status == type).ToListAsync();
        }

        public async Task<ICollection<ReportPost>> GetReportsPostByPostIdAsync(Guid postId)
        {
            return await _context.ReportPosts.Where(x => x.ReportedPostId == postId).ToListAsync();
        }
    }
}
