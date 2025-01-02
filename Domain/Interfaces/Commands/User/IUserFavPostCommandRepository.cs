using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Commands.User
{
    public interface IUserFavPostCommandRepository
    {
        Task CreateUserFavPostAsync(UserFavouritePost userFavPost);
        Task DeleteUserFavPostAsync(Guid id);
    }
}
