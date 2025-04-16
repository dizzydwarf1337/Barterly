using Domain.Entities.Posts;
using Domain.Enums;
using Domain.Interfaces.Queries.Post;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using Persistence.Repositories.Queries;
using System.Security.Cryptography.X509Certificates;

public class PostQueryRepository : BaseQueryRepository<BarterlyDbContext>, IPostQueryRepository
{
    public PostQueryRepository(BarterlyDbContext context) : base(context) { }

    public async Task<ICollection<Post>> GetAllPostsAsync()
    {
        return await _context.Posts.ToListAsync();
    }

    public async Task<Post> GetPostByIdAsync(Guid postId)
    {
        var post = await _context.Posts.FindAsync(postId);
        if (post == null)
            throw new KeyNotFoundException($"Post with id {postId} doesn't exist");
        return post;
    }


    public async Task<ICollection<Post>> GetPostsByCreatedAtAsync(DateTime createdAt)
    {
        return await _context.Posts
            .Where(x => x.CreatedAt > createdAt)
            .ToListAsync();
    }

    public async Task<ICollection<Post>> GetPostsByOwnerIdAsync(Guid ownerId)
    {
        return await _context.Posts
        .Where(x => x.OwnerId == ownerId)
            .ToListAsync();
    }

    public async Task<ICollection<Post>> GetFiltredPostsAsync(int? pageCount, int? page, Guid? categoryId = null, Guid? subCategoryId = null, string? city = null, string? region = null)
    {
        var postsQuery = _context.Posts
            .Where(p => !categoryId.HasValue || p.SubCategory.CategoryId == categoryId.Value)
            .Where(p => !subCategoryId.HasValue || p.SubCategoryId == subCategoryId.Value)
            .Where(p => string.IsNullOrEmpty(city) || p.City.Contains(city))
            .Where(p => string.IsNullOrEmpty(region) || p.Region.Contains(region))
            .Skip((page ?? 1 - 1) * (pageCount ?? 10)) 
            .Take(pageCount ?? 10); 
        return await postsQuery.ToListAsync();
    }
    public async Task<ICollection<Post>> GetUserFavouritePosts(Guid userId)
    {
        return await _context.UserFavouritePosts
            .Where(x => x.UserId == userId)
        .Select(x => x.Post)
            .ToListAsync();
    }


    public async Task<ICollection<Post>> GetUserPromotedPostsAsync(Guid userId)
    {
        return await _context.Posts
            .Include(x => x.Promotion)
            .Where(x => x.OwnerId == userId && x.Promotion.Type != PostPromotionType.None)
            .ToListAsync();
    }
    public async Task<ICollection<Post>> GetFeed(string categories, string cities, int pageCount, int page)
    {
        return await _context.Posts
            .Where(x => categories.Contains(x.SubCategory.Category.NamePL )||categories.Contains(x.SubCategory.Category.NameEN))  
            .Where(x => cities.Contains(x.City))  
            .Skip((page - 1) * pageCount)  
            .Take(pageCount)
            .ToListAsync();
    }

    public async Task<ICollection<Post>> GetPromotedPosts(int count, string? categories, string? cities)
    {
        if (count <= 0) throw new ArgumentOutOfRangeException("Wrong Input");

        var posts = await _context.Posts
            .Where(x => x.Promotion.Type == PostPromotionType.Top)
            .Where(x => categories.Contains(x.SubCategory.Category.NamePL) || categories.Contains(x.SubCategory.Category.NameEN))
            .Where(x => cities.Contains(x.City))
            .OrderBy(x => x.VisitedPosts.Count)
            .Take(count >= 3 ? count * 2 / 3 : (count == 2 ? 2 : 1))
            .ToListAsync();

        if (count > 2)
        {
            var highlightedPosts = await _context.Posts
                .Where(x => x.Promotion.Type == PostPromotionType.Highlight)
                .Where(x => categories.Contains(x.SubCategory.Category.NameEN) || categories.Contains(x.SubCategory.Category.NamePL))
                .Where(x => cities.Contains(x.City))
                .OrderBy(x => x.VisitedPosts.Count)
                .Take(count / 3)
                .ToListAsync();
            posts.AddRange(highlightedPosts);
        }

        return posts;
    }
}
