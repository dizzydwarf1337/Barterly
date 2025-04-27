
using Domain.Entities.Users;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Commands.User;
using Persistence.Database;

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
            var userFavPost = await _context.UserFavouritePosts.FindAsync(id) ?? throw new EntityNotFoundException("UserFavPost");
            _context.UserFavouritePosts.Remove(userFavPost);
            await _context.SaveChangesAsync();
        }
    }
}
