using Domain.Interfaces.Commands.User;
using Domain.Interfaces.Queries.User;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.User
{
    public class UserRepository : BaseRepository, IUserCommandRepository, IUserQueryRepository
    {
        public UserRepository(BarterlyDbContext context) : base(context) { }

        public async Task AddUserAsync(Domain.Entities.User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUser(Guid userId)
        {
            var user = await GetUserAsync(userId);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<Domain.Entities.User>> GetNewUsersAsync(DateTime date)
        {
            return await _context.Users.Where(x => DateTime.Compare(x.CreatedAt,date) >= 0).ToListAsync();
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
            return await _context.Users.Where(x=>x.City.Equals(city, StringComparison.OrdinalIgnoreCase)).ToListAsync();
        }

        public async Task<ICollection<Domain.Entities.User>> GetUsersByCountryAsync(string country)
        {
            return await _context.Users.Where(x => x.Country.Equals(country, StringComparison.OrdinalIgnoreCase)).ToListAsync();
        }

        public async Task UpdateUserAsync(Domain.Entities.User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task UploadPicture(Guid id,string PicPath)
        {
            var User = await GetUserAsync(id);
            User.ProfilePicturePath = PicPath;
            await _context.SaveChangesAsync();
        }
    }
}
