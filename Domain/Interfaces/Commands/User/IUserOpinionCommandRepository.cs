using Domain.Entities.Users;

namespace Domain.Interfaces.Commands.User;

public interface IUserOpinionCommandRepository
{
    Task<UserOpinion> CreateUserOpinionAsync(UserOpinion userOpinion, CancellationToken token);
    Task<UserOpinion> UpdateUserOpinionAsync(Guid id, string content, int rate, CancellationToken token);
    Task SetHiddenUserOpinionAsync(Guid id, bool IsHidden, CancellationToken token);
    Task DeleteUserOpinionAsync(Guid id, CancellationToken token);
}