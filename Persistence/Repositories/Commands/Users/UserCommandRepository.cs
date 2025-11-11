using Domain.Entities.Users;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Commands.User;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories.Commands.Users;

public class UserCommandRepository : BaseCommandRepository<BarterlyDbContext>, IUserCommandRepository
{
    public UserCommandRepository(BarterlyDbContext context) : base(context)
    {
    }

    public async Task AddUserAsync(User user, CancellationToken token)
    {
        await _context.Users.AddAsync(user, token);
        await _context.SaveChangesAsync(token);
    }

    public async Task DeleteUser(Guid userId, CancellationToken token)
    {
        var userSettings = await _context.UserSettings.FirstOrDefaultAsync(x=>x.UserId == userId, token) ?? throw new EntityNotFoundException("User");
        userSettings.IsDeleted = true;
        await _context.SaveChangesAsync(token);
    }

    public async Task UpdateUserAsync(User user, CancellationToken token)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync(token);
    }

    public async Task UploadPicture(Guid id, string PicPath, CancellationToken token)
    {
        var user = await _context.Users.FindAsync(id, token) ?? throw new EntityNotFoundException("User");
        user.ProfilePicturePath = PicPath;
        await _context.SaveChangesAsync(token);
    }
}