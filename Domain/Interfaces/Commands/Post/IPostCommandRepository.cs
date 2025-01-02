using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Commands.Post
{
    public interface IPostCommandRepository
    {
        Task CreatePostAsync(Domain.Entities.Post post);
        Task UpdatePostAsync(Domain.Entities.Post post);
        Task SetHidePostAsync(Guid postId, bool IsHidden);
        Task DeletePostAsync(Guid postId);
    }
}
