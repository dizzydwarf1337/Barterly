using Application.DTOs;
using Application.Services;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces
{
    public interface ILogService
    {
        Task CreateLogAsync(string message, LogType? logType,string? StackTrace, Guid? postId, Guid? userId);
        Task DeleteLogAsync(Guid logId);
        Task DeleteUserLogsAsync(Guid userId);
        Task DeletePostLogsAsync(Guid postId);
        Task<ICollection<LogDto>> GetLogsPaginatedAsync(int PageSize,int PageNum);
        Task<ICollection<LogDto>> GetUserLogsAsync(Guid userId);
        Task<ICollection<LogDto>> GetPostLogsAsync(Guid postId);
    }
}
