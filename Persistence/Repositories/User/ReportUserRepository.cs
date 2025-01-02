using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Commands.User;
using Domain.Interfaces.Queries.User;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.User
{
    public class ReportUserRepository : BaseRepository, IReportUserCommandRepository, IReportUserQueryRepository
    {
        public ReportUserRepository(BarterlyDbContext context) : base(context) { }

        public async Task ChangeReportStatus(Guid id, ReportStatusType status)
        {
            var report = await GetReportUserByIdAsync(id);
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
            var report = await GetReportUserByIdAsync(reportId);
            _context.ReportUsers.Remove(report);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<ReportUser>> GetAllUsersReportsAsync()
        {
            return await _context.ReportUsers.ToListAsync();
        }

        public async Task<ICollection<ReportUser>> GetReportsUserByUserIdAsync(Guid userId)
        {
            return await _context.ReportUsers.Where(x=>x.ReportedUserId == userId).ToListAsync();
        }

        public async Task<ReportUser> GetReportUserByIdAsync(Guid reportId)
        {
            return await _context.ReportUsers.FindAsync(reportId) ?? throw new Exception("Report user by report id not found");
        }

        public async Task<ICollection<ReportUser>> GetReportUsersByAuthorIdAsync(Guid authorId)
        {
            return await _context.ReportUsers.Where(x => x.AuthorId == authorId).ToListAsync();
        }

        public async Task<ICollection<ReportUser>> GetReportUsersByTypeAsync(ReportStatusType type)
        {
            return await _context.ReportUsers.Where(x => x.Status == type).ToListAsync();
        }
    }
}
