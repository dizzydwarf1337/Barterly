using Application.DTOs;
using Application.ServiceInterfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Commands.General;
using Domain.Interfaces.Queries.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class SessionService : ISessionService
    {
        private readonly ISessionCommandRepository _sessionCommandRepository;
        private readonly ISessionQueryRepository _sessionQueryRepository;
        private readonly IMapper _mapper;
        private readonly ILogService _logService;
        public SessionService(ISessionCommandRepository sessionCommandRepository, ISessionQueryRepository sessionQueryRepository, IMapper mapper, ILogService logService)
        {
            _sessionCommandRepository = sessionCommandRepository;
            _sessionQueryRepository = sessionQueryRepository;
            _mapper = mapper;
            _logService = logService;
        }
        public async Task DeleteSession(Guid sessionId)
        {
            if (sessionId == Guid.Empty) { throw new ArgumentException("Session Id is required"); }
            await _sessionCommandRepository.DeleteSessionAsync(sessionId);
            await _logService.CreateLogAsync($"Session with id: {sessionId} has expired", LogType.Information, null,null,null);
        }

        public async Task ExtendSession(Guid sessionId)
        {
            var session = await _sessionQueryRepository.GetSessionByIdAsync(sessionId);
            if (session != null && session.IsExpired)
            {
                await DeleteSession(sessionId);
                throw new Exception("Session is expired");
            }
            else if (session == null) throw new Exception("Session not found");
            else await _sessionCommandRepository.ExtendSession(sessionId);
            await _logService.CreateLogAsync("Session with id: {sessionId} has been extended", LogType.Information, null, null, null);
        }

        public async Task<SessionDto> StartSession()
        {
            var session = new Session();
            await _sessionCommandRepository.AddSessionAsync(session);
            await _logService.CreateLogAsync($"New Session with id: {session.Id} has been started",LogType.Information, null, null, null);
            return _mapper.Map<SessionDto>(session);
        }
    }
}
