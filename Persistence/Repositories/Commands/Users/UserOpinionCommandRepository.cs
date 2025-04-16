using Domain.Entities.Users;
using Domain.Interfaces.Commands.User;
using Persistence.Database;

namespace Persistence.Repositories.Commands.Users
{
    public class UserOpinionCommandRepository : BaseCommandRepository<BarterlyDbContext>, IUserOpinionCommandRepository
    {
        public UserOpinionCommandRepository(BarterlyDbContext context) : base(context)
        {
        }

        public async Task CreateUserOpinionAsync(UserOpinion userOpinion)
        {
            await _context.UserOpinions.AddAsync(userOpinion);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserOpinionAsync(Guid id)
        {
            var UserOpinion = await _context.UserOpinions.FindAsync(id) ?? throw new Exception("UserOpinion not found");
            _context.UserOpinions.Remove(UserOpinion);
            await _context.SaveChangesAsync();
        }

        public async Task SetHiddenUserOpinionAsync(Guid id, bool IsHidden)
        {
            var UserOpinion = await _context.UserOpinions.FindAsync(id) ?? throw new Exception("UserOpinion not found");
            UserOpinion.IsHidden = IsHidden;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserOpinionAsync(UserOpinion userOpinion)
        {
            _context.UserOpinions.Update(userOpinion);
            await _context.SaveChangesAsync();
        }
    }
}
