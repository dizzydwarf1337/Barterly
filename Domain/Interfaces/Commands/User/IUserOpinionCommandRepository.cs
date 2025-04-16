using Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Commands.User
{
    public interface IUserOpinionCommandRepository
    {
        Task CreateUserOpinionAsync(UserOpinion userOpinion);
        Task UpdateUserOpinionAsync(UserOpinion userOpinion);
        Task SetHiddenUserOpinionAsync(Guid id, bool IsHidden);
        Task DeleteUserOpinionAsync(Guid id);
    }
}
