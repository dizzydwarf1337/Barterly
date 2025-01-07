using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces
{
    public interface ITokenService
    {
        Task<string> GenerateAuthToken(Guid userId);
        Task<string> GenerateEmailConfirmationToken(Guid userId);
        Task DeleteTokenByUserId(Guid userId, string tokenType);
        Task<bool> CheckUserToken(Guid userId, string tokenType, string token);
    }
}
