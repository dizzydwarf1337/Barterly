using Application.DTOs.General;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.Common;
using Domain.Enums.Common;
using Domain.Interfaces.Commands.General;
using Domain.Interfaces.Queries.General;

namespace Application.Services;

public class LogService : ILogService
{
    private readonly ILogCommandRepository _logCommandRepository;
    private readonly ILogQueryRepository _logQueryRepository;
    private readonly IMapper _mapper;

    public LogService(ILogCommandRepository logCommandRepository, ILogQueryRepository logQueryRepository,
        IMapper mapper)
    {
        _logCommandRepository = logCommandRepository;
        _logQueryRepository = logQueryRepository;
        _mapper = mapper;
    }

    public async Task CreateLogAsync(LogDto logDto, CancellationToken token)
    {
        var log = _mapper.Map<Log>(logDto);
        await _logCommandRepository.CreateLogAsync(log, token);
    }

    public async Task CreateLogAsync(string message, CancellationToken token, LogType? logType = LogType.None,
        string? stackTrace = "", Guid? postId = null, Guid? userId = null)
    {
        var log = new Log
        {
            Message = message,
            LogType = logType ?? LogType.None,
            StackTrace = stackTrace ?? "",
            PostId = postId ?? Guid.Empty,
            UserId = userId ?? Guid.Empty
        };
        await _logCommandRepository.CreateLogAsync(log, token);
    }

    public async Task DeleteLogAsync(Guid logId, CancellationToken token)
    {
        await _logCommandRepository.DeleteLogAsync(logId, token);
    }

    public async Task DeletePostLogsAsync(Guid postId, CancellationToken token)
    {
        await _logCommandRepository.DeletePostsLogs(postId, token);
    }

    public async Task DeleteUserLogsAsync(Guid userId, CancellationToken token)
    {
        await _logCommandRepository.DeleteUsersLogs(userId, token);
    }
}