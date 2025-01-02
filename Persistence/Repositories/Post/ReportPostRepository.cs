using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Commands.Post;
using Domain.Interfaces.Queries.Post;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.Post
{
    public class ReportPostRepository : BaseRepository, IReportPostCommandRepository, IReportPostQueryRepository
    {
        public ReportPostRepository(BarterlyDbContext context) : base(context) { }
        public async Task ChangeReportPostStatusAsync(Guid id, ReportStatusType status)
        {
            var report = await GetReportPostByIdAsync(id);
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
            var report = await GetReportPostByIdAsync(reportPostId);
            _context.ReportPosts.Remove(report);
            await _context.SaveChangesAsync();
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
            return await _context.ReportPosts.Where(x=>x.AuthorId == authorId).ToListAsync();
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
