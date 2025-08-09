using Domain.Entities.Users;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Commands.User;
using Persistence.Database;

namespace Persistence.Repositories.Commands.Users;

public class UserFavPostCommandRepository : BaseCommandRepository<BarterlyDbContext>, IUserFavPostCommandRepository
{
    public UserFavPostCommandRepository(BarterlyDbContext context) : base(context)
    {
    }

    public async Task CreateUserFavPostAsync(UserFavouritePost userFavPost, CancellationToken token)
    {
        await _context.UserFavouritePosts.AddAsync(userFavPost, token);
        await _context.SaveChangesAsync(token);
    }

    public async Task DeleteUserFavPostAsync(Guid id, CancellationToken token)
    {
        var userFavPost = await _context.UserFavouritePosts.FindAsync(id, token) ??
                          throw new EntityNotFoundException("UserFavPost");
        _context.UserFavouritePosts.Remove(userFavPost);
        await _context.SaveChangesAsync(token);
    }
}