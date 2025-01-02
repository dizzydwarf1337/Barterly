using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Queries.User
{
    public interface IUserOpinionQueryRepository
    {
        Task<UserOpinion> GetUserOpinionByIdAsync(Guid id);
        Task<ICollection<UserOpinion>> GetUserOpinionsAsync();
        Task<ICollection<UserOpinion>> GetUserOpinionsByUserIdAsync(Guid userId);
        Task<ICollection<UserOpinion>> GetUserOpinionsPaginated(int page, int pageSize);
        Task<ICollection<UserOpinion>> GetUserOpinionsByAuthorIdAsync(Guid authorId);
    }
}
