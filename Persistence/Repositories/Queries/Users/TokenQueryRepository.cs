using Domain.Enums;
using Domain.Interfaces.Queries.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
               ?? throw new Exception("Token does not exist or have been deleted");
        }
    }
}
