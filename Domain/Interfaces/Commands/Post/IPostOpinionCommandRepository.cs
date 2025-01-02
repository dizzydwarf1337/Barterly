using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Commands.Post
{
    public interface IPostOpinionCommandRepository
    {
        Task CreatePostOpinionAsync(PostOpinion opinion);
        Task UpdatePostOpinionAsync(PostOpinion opinion);
        Task SetHiddenPostOpinionAsync(Guid id,bool value);
        Task DeletePostOpinionAsync(Guid id);
    }
}
