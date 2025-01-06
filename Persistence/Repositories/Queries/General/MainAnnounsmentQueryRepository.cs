using Domain.Entities;
using Domain.Interfaces.Queries.General;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.Queries.General
{
    public class MainAnnounsmentQueryRepository : BaseQueryRepository<BarterlyDbContext>, IMainAnnounsmentQueryRepository
    {
        public MainAnnounsmentQueryRepository(BarterlyDbContext context) : base(context)
        {
        }

        public async Task<List<MainAnnounsment>> GetAllMainAnnounsmentsAsync()
        {
            return await _context.MainAnnounsments.ToListAsync();
        }

        public async Task<MainAnnounsment> GetMainAnnounsmentByIdAsync(Guid id)
        {
            return await _context.MainAnnounsments.FindAsync(id) ?? throw new Exception($"MainAnnounsment with id {id} not found.");
        }
    }
}
