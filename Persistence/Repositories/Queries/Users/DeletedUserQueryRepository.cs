using Domain.Entities;
using Domain.Interfaces.Queries.User;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.Queries.Users
{
    public class DeletedUserQueryRepository : BaseQueryRepository<BarterlyDbContext>, IDeletedUserQueryRepository
    {
        public DeletedUserQueryRepository(BarterlyDbContext context) : base(context)
        {
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
