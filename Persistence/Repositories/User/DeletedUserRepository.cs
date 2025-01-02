using Domain.Entities;
using Domain.Interfaces.Commands.User;
using Domain.Interfaces.Queries.User;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.User
{
    public class DeletedUserRepository : BaseRepository, IDeletedUserCommandRepository, IDeletedUserQueryRepository
    {
        public DeletedUserRepository(BarterlyDbContext context) : base(context) { }

        public async Task CreateDeletedUserAsync(DeletedUser deletedUser)
        {
            _context.DeletedUsers.Add(deletedUser);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDeletedUserAsync(Guid id)
        {
            var deletedUser = await GetDeletedUserByIdAsync(id);
            _context.DeletedUsers.Remove(deletedUser);
            await _context.SaveChangesAsync();
        }

        public async Task<DeletedUser> GetDeletedUserByIdAsync(Guid id)
        {
            return await _context.DeletedUsers.FindAsync(id) ?? throw new Exception("Deleted User not found");
        }

        public async Task<ICollection<DeletedUser>> GetDeletedUsersAsync()
        {
            return await _context.DeletedUsers.ToListAsync();
        }
    }
}
