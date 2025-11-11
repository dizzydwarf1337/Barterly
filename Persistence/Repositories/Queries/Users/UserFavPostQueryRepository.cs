using Domain.Entities.Users;
using Domain.Enums.Posts;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Queries.User;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories.Queries.Users;

public class UserFavPostQueryRepository : BaseQueryRepository<BarterlyDbContext>, IUserFavPostQueryRepository
{
    public UserFavPostQueryRepository(BarterlyDbContext context) : base(context)
    {
    }

    public async Task<UserFavouritePost> GetUserFavPostByIdAsync(Guid postId, Guid userId, CancellationToken token)
    {
        return await _context.UserFavouritePosts.Where(x=>x.PostId == postId && x.UserId == userId).FirstOrDefaultAsync(token) ??
               throw new EntityNotFoundException("User favourite post");
    }

    public IQueryable<UserFavouritePost> GetUserFavPostsAsync()
    {
        return _context.UserFavouritePosts;
    }

    public async Task<ICollection<UserFavouritePost>> GetUserFavPostsByUserIdAsync(Guid userId,
        CancellationToken token)
    {
        return await _context.UserFavouritePosts.Where(x => x.UserId == userId && x.Post.PostSettings.postStatusType == PostStatusType.Published).ToListAsync(token);
    }
}