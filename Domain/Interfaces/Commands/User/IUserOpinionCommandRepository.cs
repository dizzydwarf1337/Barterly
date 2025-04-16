using Domain.Entities.Users;

namespace Domain.Interfaces.Commands.User
{
    public interface IUserOpinionCommandRepository
    {
        Task CreateUserOpinionAsync(UserOpinion userOpinion);
        Task UpdateUserOpinionAsync(UserOpinion userOpinion);
        Task SetHiddenUserOpinionAsync(Guid id, bool IsHidden);
        Task DeleteUserOpinionAsync(Guid id);
    }
}
