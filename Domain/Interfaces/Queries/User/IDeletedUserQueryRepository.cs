using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Queries.User
{
    public interface IDeletedUserQueryRepository
    {
        Task<DeletedUser> GetDeletedUserByIdAsync(Guid id);
        Task<ICollection<DeletedUser>> GetDeletedUsersAsync();
    }
}
