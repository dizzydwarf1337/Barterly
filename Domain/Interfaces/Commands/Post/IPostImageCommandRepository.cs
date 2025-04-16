using Domain.Entities.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Commands.Post
{
    public interface IPostImageCommandRepository
    {
        Task CreatePostImageAsync(PostImage postImage);
        Task DeletePostImageAsync(Guid id);
    }
}
