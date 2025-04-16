using Domain.Entities.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Queries.Post
{
    public interface IPostOpinionQueryRepository
    {
        Task<PostOpinion> GetPostOpinionByIdAsync(Guid id);
        Task<ICollection<PostOpinion>> GetPostOpinionsByPostIdAsync(Guid postId);
        Task<ICollection<PostOpinion>> GetPostOpinionsByAuthorIdAsync(Guid userId);
        Task<ICollection<PostOpinion>> GetPostOpinionsPaginatedAsync(Guid postId, int page, int pageSize);
        Task<ICollection<PostOpinion>> GetPostOpinionsAsync();
    }
}
