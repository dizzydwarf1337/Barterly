using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Commands.General
{
    public interface ISessionFavouriteCommandRepository
    {
        Task AddSessionFavouriteAsync(SessionFavouritePost sessionFavourite);
        Task DeleteSessionFavourite(Guid sessionFavouriteId);
    }
}
