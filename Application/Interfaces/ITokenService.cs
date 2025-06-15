using Domain.Enums.Common;

namespace Application.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateEmailConfirmationToken(string userMail);
        Task<string> GeneratePasswordResetToken(string userMail);
        Task DeleteAuthToken(string userMail);
        Task<bool> CheckUserToken(string userMail, TokenType tokenType, string token);
        Task<string> GetLoginToken(Guid userId);
    }
}
