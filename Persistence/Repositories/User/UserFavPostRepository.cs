using Domain.Entities;
using Domain.Interfaces.Commands.User;
using Domain.Interfaces.Queries.User;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.User
{
    public class UserFavPostRepository : BaseRepository, IUserFavPostCommandRepository, IUserFavPostQueryRepository
    {
        public UserFavPostRepository(BarterlyDbContext context) : base(context){}

        public async Task CreateUserFavPostAsync(UserFavouritePost userFavPost)
        {
            await _context.UserFavouritePosts.AddAsync(userFavPost);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserFavPostAsync(Guid id)
        {
            var userFavPost = await GetUserFavPostByIdAsync(id);
            _context.UserFavouritePosts.Remove(userFavPost);
            await _context.SaveChangesAsync();
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
