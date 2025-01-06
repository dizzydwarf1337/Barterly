using Application.ServiceInterfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserActivityService : IUserActivityService
    {
        public Task CreateUserActivity(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUserActivity(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<UserActivitySummary> SummarizeUserActivity(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserActivity(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
