using Domain.Entities.Users;

namespace Domain.Interfaces.Queries.User
{
    public interface IUserOpinionQueryRepository
    {
        Task<UserOpinion> GetUserOpinionByIdAsync(Guid id);
        Task<ICollection<UserOpinion>> GetUserOpinionsAsync();
        Task<ICollection<UserOpinion>> GetUserOpinionsByUserIdAsync(Guid userId);
        Task<ICollection<UserOpinion>> GetUserOpinionsPaginated(Guid userId, int page, int pageSize);
        Task<ICollection<UserOpinion>> GetUserOpinionsByAuthorIdAsync(Guid authorId);
    }
}
