using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Commands.User
{
    public interface IDeletedUserCommandRepository
    {
        Task CreateDeletedUserAsync(DeletedUser deletedUser);
        Task DeleteDeletedUserAsync(Guid id);
    }
}
