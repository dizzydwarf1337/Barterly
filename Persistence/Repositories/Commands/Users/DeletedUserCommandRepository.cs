using Domain.Entities;
using Domain.Interfaces.Commands.User;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.Commands.Users
{
    public class DeletedUserCommandRepository : BaseCommandRepository<BarterlyDbContext>, IDeletedUserCommandRepository
    {
        public DeletedUserCommandRepository(BarterlyDbContext context) : base(context)
        {
        }

        public async Task CreateDeletedUserAsync(DeletedUser deletedUser)
        {
            _context.DeletedUsers.Add(deletedUser);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDeletedUserAsync(Guid id)
        {
            var deletedUser = await _context.DeletedUsers.FindAsync(id) ?? throw new Exception("DeletedUser not found");
            _context.DeletedUsers.Remove(deletedUser);
            await _context.SaveChangesAsync();
        }
    }
}
