using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserActivityService
    {
        Task<UserActivitySummary> CreateUserActivity(Guid userId);
        Task DeleteUserActivity(Guid userId);
        Task<UserActivitySummary> SummarizeUserActivity(Guid userId);
        Task<UserActivitySummary> GetUserActivityByUserId(Guid userId);

    }
}
