using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Commands.Post
{
    public interface IPostCommandRepository
    {
        Task CreatePostAsync(Entities.Posts.Post post);
        Task UpdatePostAsync(Entities.Posts.Post post);
        Task SetHidePostAsync(Guid postId, bool IsHidden);
        Task DeletePostAsync(Guid postId);
    }
}
