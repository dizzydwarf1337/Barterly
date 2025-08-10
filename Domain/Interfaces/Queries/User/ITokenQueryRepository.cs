using Domain.Enums.Common;
using Microsoft.AspNetCore.Identity;

namespace Domain.Interfaces.Queries.User;

public interface ITokenQueryRepository
{
    Task<IdentityUserToken<Guid>> GetTokenByUserIdAsync(Guid userId,
        TokenType TokenType);
}