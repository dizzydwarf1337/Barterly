using Domain.Entities.Common;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Queries.General;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

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
            return await _context.MainAnnounsments.FindAsync(id) ?? throw new EntityNotFoundException($"MainAnnounsment with id {id}");
        }
    }
}
