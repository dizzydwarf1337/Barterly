using Domain.Interfaces.Commands.User;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.Commands.Users
{
    public class TokenCommandRepository : BaseCommandRepository<BarterlyDbContext>, ITokenCommandRepository
    {
        public TokenCommandRepository(BarterlyDbContext context) : base(context) { }

        public async Task AddToken(Guid userId, string token, string LoginProvider, string type)
        {
            await _context.UserTokens.AddAsync(new Microsoft.AspNetCore.Identity.IdentityUserToken<Guid> { UserId = userId, LoginProvider = LoginProvider, Value = token, Name = type });
            await _context.SaveChangesAsync();
        }

        public async Task DeleteToken(Guid userId, string type)
        {
            var token = await _context.UserTokens.FirstOrDefaultAsync(x => x.UserId == userId && x.Name.Equals(type, StringComparison.OrdinalIgnoreCase));
        }
    }
}
