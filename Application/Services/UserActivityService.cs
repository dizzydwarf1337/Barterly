using Application.Interfaces;
using Domain.Entities.Users;
using Domain.Interfaces.Commands.User;
using Domain.Interfaces.Queries.Post;
using Domain.Interfaces.Queries.User;

namespace Application.Services;

public class UserActivityService : IUserActivityService
{
    private readonly ICategoryQueryRepository _categoryQueryRepository;
    private readonly IUserActivityCommandRepository _userActivityCommandRepository;
    private readonly IUserActivityQueryRepository _userActivityQueryRepository;
    private readonly IVisitedPostQueryRepository _visitedPostQueryRepository;

    public UserActivityService(
        IUserActivityCommandRepository userActivityCommandRepository,
        IUserActivityQueryRepository userActivityQueryRepository,
        IVisitedPostQueryRepository visitedPostQueryRepository,
        ICategoryQueryRepository categoryQueryRepository
    )
    {
        _userActivityCommandRepository = userActivityCommandRepository;
        _userActivityQueryRepository = userActivityQueryRepository;
        _visitedPostQueryRepository = visitedPostQueryRepository;
        _categoryQueryRepository = categoryQueryRepository;
    }

    public async Task<UserActivitySummary> SummarizeUserActivity(Guid userId, CancellationToken token)
    {
        var userActivity = await _userActivityQueryRepository.GetUserActivityByIdAsync(userId, token);
        if (userActivity == null) userActivity = await CreateUserActivity(userId, token);

        userActivity.MostViewedCities = string.Join(",", await GetMostViewedCitiesAsync(userId, token));
        userActivity.MostViewedCategories = string.Join(",", await GetMostViewedCategoriesAsync(userId, token));
        userActivity.TotalPostsVisited =
            (await _visitedPostQueryRepository.GetVisitedPostsByUserIdAsync(userId, token)).Count;
        await _userActivityCommandRepository.UpdateUserActivitySummary(userActivity, token);
        return userActivity;
    }

    public async Task<UserActivitySummary> CreateUserActivity(Guid userId, CancellationToken token)
    {
        UserActivitySummary? userActivity;
        try
        {
            userActivity = await _userActivityQueryRepository.GetUserActivityByUserIdAsync(userId, token);
        }
        catch
        {
            userActivity = null;
        }

        if (userActivity != null) return userActivity;

        var userActivitySummary = new UserActivitySummary
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            TotalPostsVisited = 0,
            CreatedAt = DateTime.Now
        };
        await _userActivityCommandRepository.CreateUserActivitySummary(userActivitySummary, token);
        return userActivitySummary;
    }

    private async Task<ICollection<string?>> GetMostViewedCategoriesAsync(Guid userId, CancellationToken token)
    {
        var posts = await _visitedPostQueryRepository.GetVisitedPostsByUserIdAsync(userId, token);
        var categories = posts
            .GroupBy(x => x.Post.SubCategory.CategoryId)
            .OrderByDescending(g => g.Count())
            .Select(g => g.Key)
            .ToList();
        var categoriesName = new List<string?>();
        foreach (var categoryGuid in categories)
            categoriesName.Add((await _categoryQueryRepository.GetCategoryByIdAsync(categoryGuid, token)).NameEN);

        return categoriesName;
    }

    private async Task<ICollection<string?>> GetMostViewedCitiesAsync(Guid userId, CancellationToken token)
    {
        var posts = await _visitedPostQueryRepository.GetVisitedPostsByUserIdAsync(userId, token);
        var cities = posts
            .GroupBy(p => p.Post.City)
            .OrderByDescending(g => g.Count())
            .Select(g => g.Key?.ToString())
            .ToList();
        return cities;
    }
}