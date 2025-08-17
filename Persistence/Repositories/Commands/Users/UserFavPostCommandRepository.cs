using Domain.Entities.Users;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Commands.User;
using Microsoft.EntityFrameworkCore;
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

    public async Task DeleteUserFavPostAsync(Guid postId, Guid userId, CancellationToken token)
    {
        var userFavPost = await _context.UserFavouritePosts
                              .Where(x=>x.UserId == userId && x.PostId == postId)
                              .FirstOrDefaultAsync(token) 
                                ?? throw new EntityNotFoundException("UserFavPost");
        _context.UserFavouritePosts.Remove(userFavPost);
        await _context.SaveChangesAsync(token);
    }
}