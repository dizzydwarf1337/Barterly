using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Queries.User
{
    public interface IUserQueryRepository
    {
        Task<Entities.Users.User> GetUserAsync(Guid id);
        Task<ICollection<Entities.Users.User>> GetUsersAsync();
        Task<ICollection<Entities.Users.User>> GetUsersByCityAsync(string city);
        Task<ICollection<Entities.Users.User>> GetUsersByCountryAsync(string country);
        Task<ICollection<Entities.Users.User>> GetNewUsersAsync(DateTime date);
    }
}
