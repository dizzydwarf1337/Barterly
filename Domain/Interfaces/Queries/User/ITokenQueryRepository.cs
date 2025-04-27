using Domain.Enums.Common;

namespace Domain.Interfaces.Queries.User
{
    public interface ITokenQueryRepository
    {

        Task<Microsoft.AspNetCore.Identity.IdentityUserToken<Guid>> GetTokenByUserIdAsync(Guid userId, TokenType TokenType);
    }
}
