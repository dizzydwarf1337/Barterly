using Domain.Entities.Users;

namespace Domain.Interfaces.Commands.User
{
    public interface IUserOpinionCommandRepository
    {
        Task<UserOpinion> CreateUserOpinionAsync(UserOpinion userOpinion);
        Task<UserOpinion> UpdateUserOpinionAsync(Guid id, string content,int rate);
        Task SetHiddenUserOpinionAsync(Guid id, bool IsHidden);
        Task DeleteUserOpinionAsync(Guid id);
    }
}
