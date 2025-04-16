using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Commands.User
{
    public interface IUserCommandRepository
    {
        Task AddUserAsync(Entities.Users.User user);
        Task UpdateUserAsync(Entities.Users.User user);
        Task DeleteUser(Guid userId);
        Task UploadPicture(Guid id,string PicPath);
    }
}
