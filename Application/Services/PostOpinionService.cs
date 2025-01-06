using Application.DTOs;
using Application.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PostOpinionService : IOpinionService
    {
        public Task AddOpinion(OpinionDto opinion)
        {
            throw new NotImplementedException();
        }

        public Task DeleteOpinion(Guid opinionId)
        {
            throw new NotImplementedException();
        }

        public Task<OpinionDto> GetOpinionById(Guid opinionId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OpinionDto>> GetOpinions()
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<OpinionDto>> GetOpinionsByPostId(Guid postId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<OpinionDto>> GetOpinionsByUserId(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateOpinion(OpinionDto opinion)
        {
            throw new NotImplementedException();
        }
    }
}
