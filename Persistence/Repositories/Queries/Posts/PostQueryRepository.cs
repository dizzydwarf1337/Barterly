using Azure;
using Domain.Entities.Posts;
using Domain.Enums.Posts;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Queries.Post;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using Persistence.Repositories.Queries;
using System.Reflection.Metadata.Ecma335;

public class PostQueryRepository : BaseQueryRepository<BarterlyDbContext>, IPostQueryRepository
{
    public PostQueryRepository(BarterlyDbContext context) : base(context) { }


    public async Task<Post> GetPostById(Guid postId)
    {
        var post = await _context.Posts.Include(x=>x.PostImages).Include(x=>x.PostOpinions).Include(x=>x.Promotion).Include(x=>x.SubCategory).FirstOrDefaultAsync(x=>x.Id==postId);
        if (post == null || post.PostSettings.IsHidden || post.PostSettings.IsDeleted)
            throw new EntityNotFoundException($"Post with id {postId}");
        return post;
    }

    public async Task<Post> GetPostByIdAdmin(Guid postId)
    {
        var post = await _context.Posts.FindAsync(postId) ?? throw new EntityNotFoundException("Post");

        return post;
    }

    public async Task<ICollection<Post>> GetPostsByOwnerIdPaginated(Guid ownerId, int pageCount, int page, Guid? currentUserId = null)
    {
        var query = _context.Posts.Where(x => x.OwnerId == ownerId);

        if (currentUserId == null || currentUserId != ownerId)
        {
            query = query.Where(x => !x.PostSettings.IsDeleted && !x.PostSettings.IsHidden);
        }
        return await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageCount)
            .Take(pageCount)
            .ToListAsync();
    }

    public async Task<ICollection<Post>> GetFiltredPostsAsync(int? pageCount, int? page, Guid? subCategoryId = null, string? city = null, string? region = null)
    {
        var postsQuery = _context.Posts.AsQueryable();

        if (subCategoryId.HasValue)
        {
            postsQuery = postsQuery.Where(p => p.SubCategoryId == subCategoryId.Value);
        }

        if (!string.IsNullOrEmpty(city))
        {
            postsQuery = postsQuery.Where(p => p.City.Contains(city));
        }

        if (!string.IsNullOrEmpty(region))
        {
            postsQuery = postsQuery.Where(p => p.Region.Contains(region));
        }

        return await postsQuery
            .Where(x => !x.PostSettings.IsDeleted && !x.PostSettings.IsHidden)
            .OrderByDescending(x => x.CreatedAt)
            .Skip(((page ?? 1) - 1) * (pageCount ?? 10))
            .Take(pageCount ?? 10)
            .ToListAsync();
    }
    public async Task<ICollection<Post>> GetUserFavouritePostsPaginated(Guid userId, int pageCount, int page)
    {
        return await _context.UserFavouritePosts
            .OrderByDescending(x=> x.CreatedAt)
            .Where(x => x.UserId == userId)
            .Select(x => x.Post)
            .Where(x => !x.PostSettings.IsDeleted && !x.PostSettings.IsHidden)
            .Skip(( page - 1) * pageCount)
            .ToListAsync();
    }

    public async Task<ICollection<Post>> GetFeed(List<string>? categories, List<string>? cities, int pageCount, int page)
    {
        var query = _context.Posts.AsQueryable();

        if (categories is { Count: > 0 })
        {
            query = query.Where(x =>
                categories.Contains(x.SubCategory.Category.NamePL) ||
                categories.Contains(x.SubCategory.Category.NameEN));
        }

        if (cities is { Count: > 0 })
        {
            query = query.Where(x => cities.Contains(x.City));
        }

        return await query
            .Where(x=> !x.PostSettings.IsDeleted && !x.PostSettings.IsHidden)
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageCount)
            .Take(pageCount)
            .ToListAsync();
    }

    public async Task<ICollection<Post>> GetPromotedPosts(int count, List<string>? categories, List<string>? cities)
    {
        if (count < 0) throw new InvalidDataException("Invalid count");

        var topQuery = _context.Posts
            .Where(x => x.Promotion.Type == PostPromotionType.Top && (!x.PostSettings.IsDeleted && !x.PostSettings.IsHidden));

        if (categories is { Count: > 0 })
        {
            topQuery = topQuery.Where(x =>
                categories.Contains(x.SubCategory.Category.NamePL) ||
                categories.Contains(x.SubCategory.Category.NameEN));
        }

        if (cities is { Count: > 0 })
        {
            topQuery = topQuery.Where(x => cities.Contains(x.City));
        }

        var posts = await topQuery
            .OrderBy(x => x.VisitedPosts.Count)
            .Take(count >= 3 ? count * 2 / 3 : (count == 2 ? 2 : 1))
            .ToListAsync();

        if (count > 2)
        {
            var highlightQuery = _context.Posts
                .Where(x => x.Promotion.Type == PostPromotionType.Highlight && (!x.PostSettings.IsDeleted && !x.PostSettings.IsHidden));

            if (categories is { Count: > 0 })
            {
                highlightQuery = highlightQuery.Where(x =>
                    categories.Contains(x.SubCategory.Category.NamePL) ||
                    categories.Contains(x.SubCategory.Category.NameEN));
            }

            if (cities is { Count: > 0 })
            {
                highlightQuery = highlightQuery.Where(x => cities.Contains(x.City));
            }

            var highlightedPosts = await highlightQuery
                .OrderBy(x => x.VisitedPosts.Count)
                .Take(count / 3)
                .ToListAsync();

            posts.AddRange(highlightedPosts);
        }

        return posts;
    }

    public async Task<ICollection<Post>> GetPopularPosts(int count, string? city)
    {
        var query = _context.Posts.AsQueryable();

        if (!string.IsNullOrWhiteSpace(city))
        {
            query = query.Where(p => p.City == city);
        }

        return await query
            .Where(x => x.CreatedAt >= DateTime.UtcNow.AddDays(-10) && (!x.PostSettings.IsDeleted && !x.PostSettings.IsHidden))
            .OrderByDescending(p => p.VisitedPosts.Count)
            .Take(count)
            .ToListAsync();
    }
}
