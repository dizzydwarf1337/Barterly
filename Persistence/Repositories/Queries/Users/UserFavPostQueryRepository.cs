using Domain.Entities;
using Domain.Interfaces.Queries.User;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.Queries.Users
{
    public class UserFavPostQueryRepository : BaseQueryRepository<BarterlyDbContext>, IUserFavPostQueryRepository
    {
        public UserFavPostQueryRepository(BarterlyDbContext context) : base(context)
        {
        }

        public async Task<UserFavouritePost> GetUserFavPostByIdAsync(Guid id)
        {
            return await _context.UserFavouritePosts.FindAsync(id) ?? throw new Exception("User favourite post not found");
        }

        public async Task<ICollection<UserFavouritePost>> GetUserFavPostsAsync()
        {
            return await _context.UserFavouritePosts.ToListAsync();
        }

        public async Task<ICollection<UserFavouritePost>> GetUserFavPostsByUserIdAsync(Guid userId)
        {
            return await _context.UserFavouritePosts.Where(x => x.UserId == userId).ToListAsync();
        }
    }
}
