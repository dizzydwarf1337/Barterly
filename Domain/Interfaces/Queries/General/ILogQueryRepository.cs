using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Queries.General
{
    public interface ILogQueryRepository
    {
        Task<Log> GetLogByIdAsync(Guid id);
        Task<ICollection<Log>> GetLogsAsync();
        Task<ICollection<Log>> GetLogsByUserIdAsync(Guid userId);
        Task<ICollection<Log>> GetLogsByPostIdAsync(Guid postId);
    }
}
