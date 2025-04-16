using Domain.Enums;

namespace Domain.Interfaces.Commands.User
{
    public interface ITokenCommandRepository
    {
        Task AddToken(Guid userId, string token, string LoginProvider, TokenType tokenType);
        Task DeleteToken(string token);
        Task DeleteToken(Guid userId, TokenType tokenType);
    }
}
