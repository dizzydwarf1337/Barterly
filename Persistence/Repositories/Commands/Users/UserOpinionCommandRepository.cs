using Domain.Entities.Common;
using Domain.Entities.Users;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Commands.User;
using Persistence.Database;

namespace Persistence.Repositories.Commands.Users
{
    public class UserOpinionCommandRepository : BaseCommandRepository<BarterlyDbContext>, IUserOpinionCommandRepository
    {
        public UserOpinionCommandRepository(BarterlyDbContext context) : base(context)
        {
        }

        public async Task<UserOpinion> CreateUserOpinionAsync(UserOpinion userOpinion)
        {
            _ = await _context.Users.FindAsync(userOpinion.UserId) ?? throw new EntityNotFoundException("User");

            await _context.UserOpinions.AddAsync(userOpinion);
            await _context.SaveChangesAsync();
            return userOpinion;
        }

        public async Task DeleteUserOpinionAsync(Guid id)
        {
            var UserOpinion = await _context.UserOpinions.FindAsync(id) ?? throw new EntityNotFoundException("UserOpinion");
            _context.UserOpinions.Remove(UserOpinion);
            await _context.SaveChangesAsync();
        }

        public async Task SetHiddenUserOpinionAsync(Guid id, bool IsHidden)
        {
            var UserOpinion = await _context.UserOpinions.FindAsync(id) ?? throw new EntityNotFoundException("UserOpinion");
            UserOpinion.IsHidden = IsHidden;
            await _context.SaveChangesAsync();
        }

        public async Task<UserOpinion> UpdateUserOpinionAsync(Guid id, string content, int rate)
        {
            var opinion = await _context.UserOpinions.FindAsync(id) ?? throw new EntityNotFoundException("UserOpinion");
            opinion.Content = content;
            opinion.Rate = rate;
            opinion.LastUpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return opinion;
        }
    }
}
