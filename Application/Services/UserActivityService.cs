using Application.Interfaces;
using Domain.Entities.Users;
using Domain.Interfaces.Commands.User;
using Domain.Interfaces.Queries.Post;
using Domain.Interfaces.Queries.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class UserActivityService : IUserActivityService
{
    private readonly ICategoryQueryRepository _categoryQueryRepository;
    private readonly IUserActivityCommandRepository _userActivityCommandRepository;
    private readonly IUserActivityQueryRepository _userActivityQueryRepository;
    private readonly IVisitedPostQueryRepository _visitedPostQueryRepository;
    private readonly ILogger<UserActivityService> _logger;

    public UserActivityService(
        IUserActivityCommandRepository userActivityCommandRepository,
        IUserActivityQueryRepository userActivityQueryRepository,
        IVisitedPostQueryRepository visitedPostQueryRepository,
        ICategoryQueryRepository categoryQueryRepository,
        ILogger<UserActivityService> logger
    )
    {
        _userActivityCommandRepository = userActivityCommandRepository;
        _userActivityQueryRepository = userActivityQueryRepository;
        _visitedPostQueryRepository = visitedPostQueryRepository;
        _categoryQueryRepository = categoryQueryRepository;
        _logger = logger;
    }

    public async Task<UserActivitySummary> SummarizeUserActivity(Guid userId, CancellationToken token)
    {
        var userActivity = await GetOrCreateUserActivity(userId, token);
        
        var visitedPosts = await _visitedPostQueryRepository.GetVisitedPostsByUserIdAsync(userId, token);
        
        userActivity.TotalPostsVisited = visitedPosts.Count;
        userActivity.MostViewedCities = string.Join(",", GetMostViewedCities(visitedPosts));
        userActivity.MostViewedCategories = string.Join(",", await GetMostViewedCategoriesAsync(visitedPosts, token));
        userActivity.UpdatedAt = DateTime.UtcNow;
        
        await _userActivityCommandRepository.UpdateUserActivitySummary(userActivity, token);

        return userActivity;
    }

    public async Task<UserActivitySummary> CreateUserActivity(Guid userId, CancellationToken token)
    {
        var userActivitySummary = new UserActivitySummary
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            TotalPostsVisited = 0,
            MostViewedCities = string.Empty,
            MostViewedCategories = string.Empty,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _userActivityCommandRepository.CreateUserActivitySummary(userActivitySummary, token);
        
        _logger.LogInformation("Created user activity summary for user {UserId}", userId);
        
        return userActivitySummary;
    }

    private async Task<UserActivitySummary> GetOrCreateUserActivity(Guid userId, CancellationToken token)
    {
        try
        {
            var userActivity = await _userActivityQueryRepository.GetUserActivityByUserIdAsync(userId, token);
            
            if (userActivity != null)
                return userActivity;

            _logger.LogInformation("User activity not found for user {UserId}, creating new one", userId);
            return await CreateUserActivity(userId, token);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error getting user activity for user {UserId}, creating new one", userId);
            return await CreateUserActivity(userId, token);
        }
    }

    private async Task<ICollection<string>> GetMostViewedCategoriesAsync(
        ICollection<VisitedPost> visitedPosts, 
        CancellationToken token)
    {
        if (!visitedPosts.Any())
            return Array.Empty<string>();
        
        var topCategoryIds = visitedPosts
            .GroupBy(x => x.Post.SubCategory.CategoryId)
            .OrderByDescending(g => g.Count())
            .Take(3)
            .Select(g => g.Key)
            .ToList();

        if (!topCategoryIds.Any())
            return Array.Empty<string>();
        
        var categories = await _categoryQueryRepository.GetCategoriesAsync().Where(x => topCategoryIds.Contains(x.Id)).ToListAsync(token);
        
        var categoryNames = topCategoryIds
            .Select(id => categories.FirstOrDefault(c => c.Id == id)?.NameEN)
            .Where(name => !string.IsNullOrEmpty(name))
            .ToList();

        return categoryNames!;
    }

    private static ICollection<string> GetMostViewedCities(ICollection<VisitedPost> visitedPosts)
    {
        if (!visitedPosts.Any())
            return Array.Empty<string>();

        var topCities = visitedPosts
            .Where(p => p.Post.City != null)
            .GroupBy(p => p.Post.City)
            .OrderByDescending(g => g.Count())
            .Take(3)
            .Select(g => g.Key!.ToString()!)
            .ToList();

        return topCities;
    }
}