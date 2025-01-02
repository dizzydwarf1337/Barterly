using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Queries.User
{
    public interface IUserFavPostQueryRepository
    {
        Task<ICollection<UserFavouritePost>> GetUserFavPostsAsync();
        Task<UserFavouritePost> GetUserFavPostByIdAsync(Guid id);
        Task<ICollection<UserFavouritePost>> GetUserFavPostsByUserIdAsync(Guid userId);
    }
}
