using Domain.Entities.Common;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Commands.General;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories.Commands.General;

public class MainAnnounsmentCommandRepository : BaseCommandRepository<BarterlyDbContext>,
    IMainAnnounsmentCommandRepository
{
    public MainAnnounsmentCommandRepository(BarterlyDbContext context) : base(context)
    {
    }

    public async Task CreateMainAnnounsmentAsync(MainAnnounsment mainAnnounsment, CancellationToken token)
    {
        await _context.MainAnnounsments.AddAsync(mainAnnounsment);
        await _context.SaveChangesAsync(token);
    }

    public async Task DeleteMainAnnounsmentAsync(Guid id, CancellationToken token)
    {
        var mainAnnounsment = await _context.MainAnnounsments.FindAsync(id, token) ??
                              throw new EntityNotFoundException($"MainAnnounsment with id {id}");
        _context.MainAnnounsments.Remove(mainAnnounsment);
        await _context.SaveChangesAsync(token);
    }

    public async Task UpdateMainAnnounsmentAsync(MainAnnounsment mainAnnounsment, CancellationToken token)
    {
        _context.MainAnnounsments.Update(mainAnnounsment);
        await _context.SaveChangesAsync(token);
    }

    public async Task UploadImageAsync(Guid id, string imageUrl, CancellationToken token)
    {
        var mainAnnounsment = await _context.MainAnnounsments.FindAsync(id, token) ??
                              throw new EntityNotFoundException($"MainAnnounsment with id {id}");
        mainAnnounsment.ImageUrl = imageUrl;
        _context.Entry(mainAnnounsment).State = EntityState.Modified;
        await _context.SaveChangesAsync(token);
    }
}