using System;
using Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Commands.User
{
    public interface IUserCommandRepository
    {
        Task AddUserAsync(Domain.Entities.User user);
        Task UpdateUserAsync(Domain.Entities.User user);
        Task DeleteUser(Guid userId);
        Task UploadPicture(Guid id,string PicPath);
    }
}
