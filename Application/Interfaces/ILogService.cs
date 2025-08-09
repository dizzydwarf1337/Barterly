using Application.DTOs.General;
using Domain.Enums.Common;

namespace Application.Interfaces;

public interface ILogService
{
    Task CreateLogAsync(LogDto logDto, CancellationToken token);

    Task CreateLogAsync(string message, CancellationToken token, LogType? logType, string? StackTrace = null,
        Guid? postId = null, Guid? userId = null);

    Task DeleteLogAsync(Guid logId, CancellationToken token);
    Task DeleteUserLogsAsync(Guid userId, CancellationToken token);
    Task DeletePostLogsAsync(Guid postId, CancellationToken token);
}