using Domain.Entities.Users;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Commands.User;
using Persistence.Database;

namespace Persistence.Repositories.Commands.Users;

public class UserOpinionCommandRepository : BaseCommandRepository<BarterlyDbContext>, IUserOpinionCommandRepository
{
    public UserOpinionCommandRepository(BarterlyDbContext context) : base(context)
    {
    }

    public async Task<UserOpinion> CreateUserOpinionAsync(UserOpinion userOpinion, CancellationToken token)
    {
        _ = await _context.Users.FindAsync(userOpinion.UserId, token) ?? throw new EntityNotFoundException("User");

        await _context.UserOpinions.AddAsync(userOpinion, token);
        await _context.SaveChangesAsync(token);
        return userOpinion;
    }

    public async Task DeleteUserOpinionAsync(Guid id, CancellationToken token)
    {
        var UserOpinion = await _context.UserOpinions.FindAsync(id, token) ??
                          throw new EntityNotFoundException("UserOpinion");
        _context.UserOpinions.Remove(UserOpinion);
        await _context.SaveChangesAsync(token);
    }

    public async Task SetHiddenUserOpinionAsync(Guid id, bool IsHidden, CancellationToken token)
    {
        var UserOpinion = await _context.UserOpinions.FindAsync(id, token) ??
                          throw new EntityNotFoundException("UserOpinion");
        UserOpinion.IsHidden = IsHidden;
        await _context.SaveChangesAsync(token);
    }

    public async Task<UserOpinion> UpdateUserOpinionAsync(Guid id, string content, int rate,
        CancellationToken token)
    {
        var opinion = await _context.UserOpinions.FindAsync(id, token) ??
                      throw new EntityNotFoundException("UserOpinion");
        opinion.Content = content;
        opinion.Rate = rate;
        opinion.LastUpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(token);
        return opinion;
    }
}