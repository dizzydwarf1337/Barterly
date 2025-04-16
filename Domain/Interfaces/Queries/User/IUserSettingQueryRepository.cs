using Domain.Entities.Users;

namespace Domain.Interfaces.Queries.User
{
    public interface IUserSettingQueryRepository
    {
        Task<UserSettings> GetUserSettingByIdAsync(Guid id);
        Task<UserSettings> GetUserSettingByUserIdAsync(Guid userId);
    }
}
