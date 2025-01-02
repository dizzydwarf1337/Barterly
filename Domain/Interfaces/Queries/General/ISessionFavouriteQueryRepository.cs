using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Queries.General
{
    public interface ISessionFavouriteQueryRepository
    {
        Task<SessionFavouritePost> GetSessionFavouritePostByIdAsync(Guid sessionFavouriteId);
        Task<ICollection<SessionFavouritePost>> GetSessionFavouritesBySessionIdAsync(Guid sessionId);
    }
}
