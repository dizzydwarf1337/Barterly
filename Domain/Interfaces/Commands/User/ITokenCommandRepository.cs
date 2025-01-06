using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Commands.User
{
    public interface ITokenCommandRepository
    {
        Task AddToken(Guid userId, string token, string LoginProvider, string type);
        Task DeleteToken(Guid userId, string type);
    }
}
