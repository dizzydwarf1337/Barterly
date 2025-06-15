
using Domain.Entities.Users;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Queries.User;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories.Queries.Users
{
    public class UserQueryRepository : BaseQueryRepository<BarterlyDbContext>, IUserQueryRepository
    {
        public UserQueryRepository(BarterlyDbContext context) : base(context)
        {
        }

        public async Task<ICollection<Guid>> GetUsersIdsAsync(DateTime date)
        {
            return await _context.Users.Where(x => DateTime.Compare(x.CreatedAt, date) >= 0 || DateTime.Compare(x.LastSeen,date) >=0).Select(x=>x.Id).ToListAsync();
        }

        public async Task<User> GetUserAsync(Guid id)
        {
            return await _context.Users.FindAsync(id) ?? throw new EntityNotFoundException("User");
        }
        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(
                x => x.NormalizedEmail == email.ToUpper()
            ) ?? throw new EntityNotFoundException("User");

        }
        public async Task<ICollection<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<ICollection<User>> GetUsersByCityAsync(string city)
        {
            return await _context.Users.Where(x => x.City!.Equals(city, StringComparison.OrdinalIgnoreCase)).ToListAsync();
        }

        public async Task<ICollection<User>> GetUsersByCountryAsync(string country)
        {
            return await _context.Users.Where(x => x.Country!.Equals(country, StringComparison.OrdinalIgnoreCase)).ToListAsync();
        }
    }
}
