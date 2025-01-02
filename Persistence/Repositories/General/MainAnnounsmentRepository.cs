using Domain.Entities;
using Domain.Interfaces.Commands.General;
using Domain.Interfaces.Queries.General;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.General
{
    public class MainAnnounsmentRepository : BaseRepository,IMainAnnounsmentCommandRepository, IMainAnnounsmentQueryRepository
    {
        public MainAnnounsmentRepository(BarterlyDbContext context):base(context) { }
        
        public async Task CreateMainAnnounsmentAsync(MainAnnounsment mainAnnounsment)
        {
            await _context.MainAnnounsments.AddAsync(mainAnnounsment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMainAnnounsmentAsync(Guid id)
        {
            var mainAnnounsment = _context.MainAnnounsments.Find(id) ?? throw new Exception($"MainAnnounsment with id {id} not found.");
            _context.MainAnnounsments.Remove(mainAnnounsment);
            await _context.SaveChangesAsync();
        }

        public async Task<List<MainAnnounsment>> GetAllMainAnnounsmentsAsync()
        {
            return await _context.MainAnnounsments.ToListAsync();
        }

        public async Task<MainAnnounsment> GetMainAnnounsmentByIdAsync(Guid id)
        {
            return await _context.MainAnnounsments.FindAsync(id) ?? throw new Exception($"MainAnnounsment with id {id} not found.");
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
