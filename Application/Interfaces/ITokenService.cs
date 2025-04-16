using Domain.Enums;

namespace Application.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateEmailConfirmationToken(string userMail);
        Task<string> GeneratePasswordResetToken(string userMail);
        Task DeleteTokenByUserMail(string userMail, TokenType tokenType);
        Task DeleteToken(string token);
        Task<bool> CheckUserToken(string userMail, TokenType tokenType, string token);
        Task<string> GetLoginToken(Guid userId);
    }
}
