
using Domain.Entities.Users;
using Domain.Interfaces.Commands.User;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.Commands.Users
{
    public class UserFavPostCommandRepository : BaseCommandRepository<BarterlyDbContext>, IUserFavPostCommandRepository
    {
        public UserFavPostCommandRepository(BarterlyDbContext context) : base(context)
        {
        }

        public async Task CreateUserFavPostAsync(UserFavouritePost userFavPost)
        {
            await _context.UserFavouritePosts.AddAsync(userFavPost);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserFavPostAsync(Guid id)
        {
            var userFavPost = await _context.UserFavouritePosts.FindAsync(id) ?? throw new Exception("UserFavPost not found");
            _context.UserFavouritePosts.Remove(userFavPost);
            await _context.SaveChangesAsync();
        }
    }
}
