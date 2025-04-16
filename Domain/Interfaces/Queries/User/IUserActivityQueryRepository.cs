using Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Queries.User
{
    public interface IUserActivityQueryRepository
    {
        Task<ICollection<UserActivitySummary>> GetUserActivitiesAsync();
        Task<UserActivitySummary> GetUserActivityByIdAsync(Guid id);
        Task<UserActivitySummary> GetUserActivityByUserIdAsync(Guid userId);
    }
}
