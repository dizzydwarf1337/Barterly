using Domain.Entities.Common;
using Domain.Interfaces.Commands.General;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.Commands.General
{
    public class MainAnnounsmentCommandRepository : BaseCommandRepository<BarterlyDbContext>, IMainAnnounsmentCommandRepository
    {
        public MainAnnounsmentCommandRepository(BarterlyDbContext context) : base(context)
        {
        }
        public async Task CreateMainAnnounsmentAsync(MainAnnounsment mainAnnounsment)
        {
            await _context.MainAnnounsments.AddAsync(mainAnnounsment);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteMainAnnounsmentAsync(Guid id)
        {
            var mainAnnounsment = await _context.MainAnnounsments.FindAsync(id) ?? throw new Exception($"MainAnnounsment with id {id} not found.");
            _context.MainAnnounsments.Remove(mainAnnounsment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMainAnnounsmentAsync(MainAnnounsment mainAnnounsment)
        {
            _context.MainAnnounsments.Update(mainAnnounsment);
            await _context.SaveChangesAsync();
        }

        public async Task UploadImageAsync(Guid id, string imageUrl)
        {
            var mainAnnounsment = await _context.MainAnnounsments.FindAsync(id) ?? throw new Exception($"MainAnnounsment with id {id} not found.");
            mainAnnounsment.ImageUrl = imageUrl;
            _context.Entry(mainAnnounsment).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
