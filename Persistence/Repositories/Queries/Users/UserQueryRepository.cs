using Domain.Entities;
using Domain.Interfaces.Queries.User;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.Queries.Users
{
    public class UserQueryRepository : BaseQueryRepository<BarterlyDbContext>, IUserQueryRepository
    {
        public UserQueryRepository(BarterlyDbContext context) : base(context)
        {
        }

        public async Task<ICollection<Domain.Entities.User>> GetNewUsersAsync(DateTime date)
        {
            return await _context.Users.Where(x => DateTime.Compare(x.CreatedAt, date) >= 0).ToListAsync();
        }

        public async Task<Domain.Entities.User> GetUserAsync(Guid id)
        {
            return await _context.Users.FindAsync(id) ?? throw new Exception("User not found");
        }

        public async Task<ICollection<Domain.Entities.User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<ICollection<Domain.Entities.User>> GetUsersByCityAsync(string city)
        {
            return await _context.Users.Where(x => x.City.Equals(city, StringComparison.OrdinalIgnoreCase)).ToListAsync();
        }

        public async Task<ICollection<Domain.Entities.User>> GetUsersByCountryAsync(string country)
        {
            return await _context.Users.Where(x => x.Country.Equals(country, StringComparison.OrdinalIgnoreCase)).ToListAsync();
        }
    }
}
