using Application.DTOs;
using Application.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class SessionService : ISessionService
    {
        public Task EndSession()
        {
            throw new NotImplementedException();
        }

        public Task ExtendSession(Guid sessionId)
        {
            throw new NotImplementedException();
        }

        public Task<SessionDto> StartSession()
        {
            throw new NotImplementedException();
        }
    }
}
