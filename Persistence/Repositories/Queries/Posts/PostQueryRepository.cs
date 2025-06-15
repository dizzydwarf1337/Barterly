using Azure;
using Domain.Entities.Posts;
using Domain.Enums.Posts;
using Domain.Exceptions.BusinessExceptions;
using Domain.Exceptions.DataExceptions;
using Domain.Interfaces.Queries.Post;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using Persistence.Repositories.Queries;
using System.Reflection.Metadata.Ecma335;

public class PostQueryRepository : BaseQueryRepository<BarterlyDbContext>, IPostQueryRepository
{
    public PostQueryRepository(BarterlyDbContext context) : base(context) { }


    public async Task<Post> GetPostById(Guid postId, Guid? userId = null, string? role = "User")
    {
        var post = await _context.Posts
            .Include(x=>x.PostSettings)
            .Include(x=>x.PostImages)
            .Include(x=>x.PostOpinions.Where(x=>!x.IsHidden))
            .Include(x=>x.Promotion)
            .Include(x=>x.SubCategory)
            .FirstOrDefaultAsync(x=>x.Id==postId);

        if (post == null || post.PostSettings == null)
        {
            throw new EntityNotFoundException($"Post with id {postId}");
        }

        if (role != "User")
        {
            return post;
        }

        if (post.PostSettings.IsDeleted)
        {
            throw new EntityNotFoundException($"Post with id {postId}");
        }

        if (post.PostSettings.IsHidden && post.OwnerId != userId)
        {
            throw new EntityNotFoundException($"Post with id {postId}");
        }
        return post;
    }


    public async Task<ICollection<Post>> GetPostsByOwnerIdPaginated(Guid ownerId, int pageCount, int page, Guid? currentUserId = null, string? role = "User")
    {
        var query = _context.Posts.Where(x => x.OwnerId == ownerId).AsQueryable();

        if (currentUserId == null || currentUserId != ownerId || role == "User")
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
            postsQuery = postsQuery.Where(p => p.City!.Contains(city));
        }

        if (!string.IsNullOrEmpty(region))
        {
            postsQuery = postsQuery.Where(p => p.Region!.Contains(region));
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
        if (pageCount < 0) throw new InvalidDataProvidedException("PageCount getFeed");
        var postsToReturn = new HashSet<Post>();
        var query = _context.Posts
            .Where(x => x.CreatedAt > DateTime.Now.AddDays(-40) && (!x.PostSettings.IsHidden && !x.PostSettings.IsDeleted))
            .Include(x => x.PostSettings)
            .Include(x => x.Promotion)
            .Include(x => x.VisitedPosts)
            .Include(x => x.SubCategory)
            .AsQueryable();
        var fullQuery = query;
        if(categories?.Count != 0 )
        {
            fullQuery = fullQuery.Where(x=>categories!.Contains(x.SubCategory.TitleEN) || categories.Contains(x.SubCategory.TitlePL));

        }
        if(cities?.Count != 0)
        {
            fullQuery = fullQuery.Where(x => cities!.Contains(x.City!));
        }
        var posts = await fullQuery.Skip((page - 1) * pageCount).Take(pageCount).ToListAsync();
        foreach(var post in posts)
        {
            postsToReturn.Add(post);
        }
        int skipMultiplier = 0;
        while (postsToReturn.Count < pageCount)
        {
            var additionalPosts = await query.Skip((pageCount-postsToReturn.Count) * skipMultiplier).Take(pageCount - postsToReturn.Count).ToListAsync();
            foreach(var post in additionalPosts)
            {
                postsToReturn.Add(post);
            }
            skipMultiplier++;
            if (skipMultiplier > 10) break;
        }
        return postsToReturn;
    }

    public async Task<ICollection<Post>> GetPromotedPosts(int count, List<string>? categories, List<string>? cities)
    {
        var postsToReturn = new HashSet<Post>();

        var query = _context.Posts
            .Include(x => x.PostSettings)
            .Where(x => x.CreatedAt > DateTime.Now.AddDays(-40) && (!x.PostSettings.IsHidden && !x.PostSettings.IsDeleted))
            .Include(x => x.Promotion)
            .Include(x => x.VisitedPosts)
            .Include(x => x.SubCategory)
            .AsQueryable();

        if (categories?.Any() == true)
            query = query.Where(x => categories.Contains(x.SubCategory.Category.NameEN )|| categories.Contains(x.SubCategory.Category.NameEN));

        if (cities?.Any() == true)
            query = query.Where(x => cities.Contains(x.City!));

        var topPostsCount = count > 2 ? (int)Math.Ceiling(count * 2.0 / 3) : 1;
        var topPosts = await query
            .Where(x => x.Promotion.Type == PostPromotionType.Top)
            .OrderBy(x => x.VisitedPosts!.Count)
            .Take(topPostsCount)
            .ToListAsync();

        foreach (var post in topPosts)
            postsToReturn.Add(post);

        if (postsToReturn.Count < count)
        {
            var remainingCount = count - postsToReturn.Count;
            var highlightPosts = await query
                .Where(x => x.Promotion.Type == PostPromotionType.Highlight)
                .OrderBy(x => x.VisitedPosts!.Count)
                .Take(remainingCount)
                .ToListAsync();

            foreach (var post in highlightPosts)
                postsToReturn.Add(post);
        }
        return postsToReturn;
    }

    public async Task<ICollection<Post>> GetPopularPosts(int count, string? city)
    {
        var popularPosts = new HashSet<Post>();

        var query = _context.Posts
            .Include(x => x.PostSettings)
            .Include(x => x.Promotion)
            .Include(x => x.VisitedPosts)
            .Include(x => x.SubCategory)
            .Where(x => x.CreatedAt > DateTime.Now.AddDays(-40) && (!x.PostSettings.IsHidden && !x.PostSettings.IsDeleted))
            .OrderByDescending(x=>x.ViewsCount)
            .AsQueryable();
        var cityQuery = query.Where(x => x.City == city);
        var posts = await cityQuery.Take(count).ToListAsync();
        int countMultiplier = 0;
        foreach (var post in posts) 
        {
            popularPosts.Add(post);    
        }
        while(popularPosts.Count < count)
        {
            var addtitionalPosts = await query.Skip(count*countMultiplier).Take(count).ToListAsync();
            foreach(var post in addtitionalPosts)
            {
                popularPosts.Add(post);
            }
            countMultiplier++;
            if (countMultiplier > 10) { break; }
        }
        return popularPosts;
    }



}
