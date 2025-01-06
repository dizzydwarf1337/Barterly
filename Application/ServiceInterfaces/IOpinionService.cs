using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces
{
    public interface IOpinionService
    {
        Task AddOpinion(OpinionDto opinion);
        Task DeleteOpinion(Guid opinionId);
        Task UpdateOpinion(OpinionDto opinion);


        Task<IEnumerable<OpinionDto>> GetOpinions();
        Task<OpinionDto> GetOpinionById(Guid opinionId);
        Task<ICollection<OpinionDto>> GetOpinionsByUserId(Guid userId);
        Task<ICollection<OpinionDto>> GetOpinionsByPostId(Guid postId);
    }
}
