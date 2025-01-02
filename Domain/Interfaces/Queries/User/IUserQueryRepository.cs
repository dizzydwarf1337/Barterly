using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Queries.User
{
    public interface IUserQueryRepository
    {
        Task<Domain.Entities.User> GetUserAsync(Guid id);
        Task<ICollection<Domain.Entities.User>> GetUsersAsync();
        Task<ICollection<Domain.Entities.User>> GetUsersByCityAsync(string city);
        Task<ICollection<Domain.Entities.User>> GetUsersByCountryAsync(string country);
        Task<ICollection<Domain.Entities.User>> GetNewUsersAsync(DateTime date);
    }
}
