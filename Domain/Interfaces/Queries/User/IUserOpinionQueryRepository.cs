using Domain.Entities.Users;

namespace Domain.Interfaces.Queries.User;

public interface IUserOpinionQueryRepository
{
    Task<UserOpinion> GetUserOpinionByIdAsync(Guid id, CancellationToken token);
    IQueryable<UserOpinion> GetUserOpinionsAsync();
    Task<ICollection<UserOpinion>> GetUserOpinionsByUserIdAsync(Guid userId, CancellationToken token);
    Task<ICollection<UserOpinion>> GetUserOpinionsByAuthorIdAsync(Guid authorId, CancellationToken token);
}