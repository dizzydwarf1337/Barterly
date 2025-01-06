using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Queries.User
{
    public interface ITokenQueryRepository
    {

        Task<Microsoft.AspNetCore.Identity.IdentityUserToken<Guid>> GetTokenByUserIdAsync(Guid userId, string TokenType);
    }
}
