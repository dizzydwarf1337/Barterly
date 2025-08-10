using Domain.Entities.Users;

namespace Domain.Interfaces.Queries.User;

public interface IUserSettingQueryRepository
{
    Task<UserSettings> GetUserSettingByIdAsync(Guid id, CancellationToken token);
    Task<UserSettings> GetUserSettingByUserIdAsync(Guid userId, CancellationToken token);
}