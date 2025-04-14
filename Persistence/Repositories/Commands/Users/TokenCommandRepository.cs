using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Commands.User;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.Commands.Users
{
    public class TokenCommandRepository : BaseCommandRepository<BarterlyDbContext>, ITokenCommandRepository
    {
        public TokenCommandRepository(BarterlyDbContext context) : base(context) { }

        public async Task AddToken(Guid userId, string token, string LoginProvider, TokenType tokenType)
        {
            await _context.UserTokens.AddAsync(new Microsoft.AspNetCore.Identity.IdentityUserToken<Guid> { UserId = userId, LoginProvider = LoginProvider, Value = token, Name=tokenType.ToString().ToLower()});
            await _context.SaveChangesAsync();
        }
        public async Task DeleteToken(string token)
        {
            var Otoken = await _context.UserTokens.FirstOrDefaultAsync(x=>x.Value.ToString()==token);
            if (Otoken == null)
            {
                throw new ArgumentNullException(nameof(Otoken));
            }
            _context.UserTokens.Remove(Otoken);
            await _context.SaveChangesAsync();

        }
        public async Task DeleteToken(Guid userId, TokenType type)
        {
            var token = await _context.UserTokens.FirstOrDefaultAsync(x => x.UserId == userId && x.Name == type.ToString().ToLower());
            if (token == null)
            {
                throw new ArgumentNullException("Token does not exist or have been deleted");
            }
            _context.UserTokens.Remove(token);
            await _context.SaveChangesAsync();
        }
    }
}
