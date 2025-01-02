using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Queries.Post
{
    public interface IPostImageQueryRepository
    {
        Task<PostImage> GetPostImageAsync(Guid id);
        Task<ICollection<PostImage>> GetPostImagesAsync();
        Task<ICollection<PostImage>> GetPostImagesByPostIdAsync(Guid postId);
    }
}
