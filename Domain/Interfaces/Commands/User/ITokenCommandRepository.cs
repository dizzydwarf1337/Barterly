using Domain.Enums.Common;

namespace Domain.Interfaces.Commands.User;

public interface ITokenCommandRepository
{
    Task AddToken(Guid userId, string token, string LoginProvider, TokenType tokenType,
        CancellationToken cancellationToken);

    Task DeleteToken(string token, CancellationToken cancellationToken);
    Task DeleteToken(Guid userId, TokenType tokenType, CancellationToken token);
}