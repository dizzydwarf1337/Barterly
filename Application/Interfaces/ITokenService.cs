using Domain.Enums.Common;

namespace Application.Interfaces;

public interface ITokenService
{
    Task<string> GenerateEmailConfirmationToken(string userMail, CancellationToken token);
    Task<string> GeneratePasswordResetToken(string userMail);
    Task DeleteAuthToken(string userMail);

    Task<bool> CheckUserToken(string userMail, TokenType tokenType, string token,
        CancellationToken cancellationToken);

    Task<string> GetLoginToken(Guid userId, CancellationToken token);
}