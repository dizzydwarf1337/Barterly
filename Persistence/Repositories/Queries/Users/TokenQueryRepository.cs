using Domain.Enums.Common;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Queries.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories.Queries.Users
{
    public class TokenQueryRepository : BaseQueryRepository<BarterlyDbContext>, ITokenQueryRepository
    {
        public TokenQueryRepository(BarterlyDbContext context) : base(context)
        {
        }

        public async Task<IdentityUserToken<Guid>> GetTokenByUserIdAsync(Guid userId, TokenType tokenType)
        {
            return await _context.UserTokens
                .FirstOrDefaultAsync(x => x.UserId == userId && x.Name.Equals(tokenType.ToString().ToLower()))
               ?? throw new EntityNotFoundException("Token");
        }
    }
}
