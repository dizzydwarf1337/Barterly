using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Commands.User
{
    public interface IUserActivityCommandRepository
    {
        Task CreateUserActivitySummary(UserActivitySummary userActivitySummary);
        Task DeleteUserActivitySummary(Guid SummaryId);
        Task UpdateUserActivitySummary(UserActivitySummary userActivitySummary);
    }
}
