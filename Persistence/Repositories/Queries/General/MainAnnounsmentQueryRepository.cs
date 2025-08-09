using Domain.Entities.Common;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Queries.General;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories.Queries.General;

public class MainAnnounsmentQueryRepository : BaseQueryRepository<BarterlyDbContext>,
    IMainAnnounsmentQueryRepository
{
    public MainAnnounsmentQueryRepository(BarterlyDbContext context) : base(context)
    {
    }

    public async Task<ICollection<MainAnnounsment>> GetAllMainAnnounsmentsAsync(CancellationToken token)
    {
        return await _context.MainAnnounsments.ToListAsync(token);
    }

    public async Task<MainAnnounsment> GetMainAnnounsmentByIdAsync(Guid id, CancellationToken token)
    {
        return await _context.MainAnnounsments.FindAsync(id, token) ??
               throw new EntityNotFoundException($"MainAnnounsment with id {id}");
    }
}