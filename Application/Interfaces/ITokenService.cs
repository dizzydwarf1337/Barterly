using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
