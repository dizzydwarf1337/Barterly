using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces
{
    public interface IUserActivityService
    {
        Task CreateUserActivity(Guid userId);
        Task UpdateUserActivity(Guid userId);
        Task DeleteUserActivity(Guid userId);
        Task<UserActivitySummary> SummarizeUserActivity(Guid userId);

    }
}
