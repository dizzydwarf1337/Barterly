using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces
{
    public interface ISessionService
    {
        Task<SessionDto> StartSession();
        Task DeleteSession(Guid sessionId);
        Task ExtendSession(Guid sessionId);

    }
}
