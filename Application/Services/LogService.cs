using Application.DTOs.General;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.Common;
using Domain.Enums.Common;
using Domain.Interfaces.Commands.General;
using Domain.Interfaces.Queries.General;


namespace Application.Services
{
    public class LogService : ILogService
    {
        private readonly ILogCommandRepository _logCommandRepository;
        private readonly ILogQueryRepository _logQueryRepository;
        private readonly IMapper _mapper;

        public LogService(ILogCommandRepository logCommandRepository, ILogQueryRepository logQueryRepository, IMapper mapper)
        {
            _logCommandRepository = logCommandRepository;
            _logQueryRepository = logQueryRepository;
            _mapper = mapper;
        }
        public async Task CreateLogAsync(LogDto logDto)
        {
            var log = _mapper.Map<Log>(logDto);
            await _logCommandRepository.CreateLogAsync(log);
        }

        public async Task CreateLogAsync(string message, LogType? logType = LogType.None, string? stackTrace = "", Guid? postId = null, Guid? userId = null)
        {
            var log = new Log
            {
                Message = message,
                LogType = logType ?? LogType.None,
                StackTrace = stackTrace ?? "",
                PostId = postId ?? Guid.Empty,
                UserId = userId ?? Guid.Empty
            };
            await _logCommandRepository.CreateLogAsync(log);
        }

        public async Task DeleteLogAsync(Guid logId)
        {
            await _logCommandRepository.DeleteLogAsync(logId);
        }

        public async Task DeletePostLogsAsync(Guid postId)
        {
            await _logCommandRepository.DeletePostsLogs(postId);
        }

        public async Task DeleteUserLogsAsync(Guid userId)
        {
            await _logCommandRepository.DeleteUsersLogs(userId);
        }

        public async Task<ICollection<LogDto>> GetLogsPaginatedAsync(int PageSize, int PageNum)
        {
            var logs = await _logQueryRepository.GetLogsPaginatedAsync(PageSize, PageNum);
            return _mapper.Map<ICollection<LogDto>>(logs);
        }

        public async Task<ICollection<LogDto>> GetPostLogsAsync(Guid postId)
        {
            var logs = await _logQueryRepository.GetLogsByPostIdAsync(postId);
            return _mapper.Map<ICollection<LogDto>>(logs);
        }

        public async Task<ICollection<LogDto>> GetUserLogsAsync(Guid userId)
        {
            var logs = await _logQueryRepository.GetLogsByUserIdAsync(userId);
            return _mapper.Map<ICollection<LogDto>>(logs);
        }
    }
}
