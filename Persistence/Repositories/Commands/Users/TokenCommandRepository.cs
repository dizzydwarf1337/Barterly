using Domain.Enums.Common;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Commands.User;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories.Commands.Users
{
    public class TokenCommandRepository : BaseCommandRepository<BarterlyDbContext>, ITokenCommandRepository
    {
        public TokenCommandRepository(BarterlyDbContext context) : base(context) { }

        public async Task AddToken(Guid userId, string token, string LoginProvider, TokenType tokenType)
        {
            await _context.UserTokens.AddAsync(new Microsoft.AspNetCore.Identity.IdentityUserToken<Guid> { UserId = userId, LoginProvider = LoginProvider, Value = token, Name = tokenType.ToString().ToLower() });
            await _context.SaveChangesAsync();
        }
        public async Task DeleteToken(string token)
        {
            var Otoken = await _context.UserTokens.FirstOrDefaultAsync(x => x.Value.ToString() == token) ?? throw new EntityNotFoundException("Token"); 
            _context.UserTokens.Remove(Otoken);
            await _context.SaveChangesAsync();

        }
        public async Task DeleteToken(Guid userId, TokenType type)
        {
            var token = await _context.UserTokens.FirstOrDefaultAsync(x => x.UserId == userId && x.Name == type.ToString().ToLower()) ?? throw new EntityNotFoundException("Token");
            _context.UserTokens.Remove(token);
            await _context.SaveChangesAsync();
        }
    }
}
