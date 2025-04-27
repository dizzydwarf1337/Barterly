
using Domain.Entities.Users;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Commands.User;
using Persistence.Database;

namespace Persistence.Repositories.Commands.Users
{
    public class UserCommandRepository : BaseCommandRepository<BarterlyDbContext>, IUserCommandRepository
    {
        public UserCommandRepository(BarterlyDbContext context) : base(context)
        {
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUser(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId) ?? throw new EntityNotFoundException("User");
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task UploadPicture(Guid id, string PicPath)
        {
            var user = await _context.Users.FindAsync(id) ?? throw new EntityNotFoundException("User"); 
            user.ProfilePicturePath = PicPath;
            await _context.SaveChangesAsync();
        }
    }
}
