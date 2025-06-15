using Application.Interfaces;
using Domain.Entities.Users;
using Domain.Interfaces.Commands.User;
using Domain.Interfaces.Queries.Post;
using Domain.Interfaces.Queries.User;

namespace Application.Services
{
    public class UserActivityService : IUserActivityService
    {
        private readonly IUserActivityCommandRepository _userActivityCommandRepository;
        private readonly IUserActivityQueryRepository _userActivityQueryRepository;
        private readonly IVisitedPostQueryRepository _visitedPostQueryRepository;
        private readonly ICategoryQueryRepository _categoryQueryRepository;

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
        public async Task<UserActivitySummary> SummarizeUserActivity(Guid userId)
        {
            var userActivity = await _userActivityQueryRepository.GetUserActivityByIdAsync(userId);
            if (userActivity == null)
            {
                userActivity = await CreateUserActivity(userId);
            }
            userActivity.MostViewedCities = String.Join(",", await GetMostViewedCitiesAsync(userId));
            userActivity.MostViewedCategories = String.Join(",", await GetMostViewedCategoriesAsync(userId));
            userActivity.TotalPostsVisited = (await _visitedPostQueryRepository.GetVisitedPostsByUserIdAsync(userId)).Count;
            await _userActivityCommandRepository.UpdateUserActivitySummary(userActivity);
            return userActivity;
        }

        public async Task<UserActivitySummary> CreateUserActivity(Guid userId)
        {
            UserActivitySummary? userActivity;
            try
            {
                userActivity = await _userActivityQueryRepository.GetUserActivityByUserIdAsync(userId);
            }
            catch
            {
                userActivity = null;
            }
            if (userActivity != null)
            {
                return userActivity;
            }
            UserActivitySummary userActivitySummary = new UserActivitySummary
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                TotalPostsVisited = 0,
                CreatedAt = DateTime.Now,
            };
            await _userActivityCommandRepository.CreateUserActivitySummary(userActivitySummary);
            return userActivitySummary;
        }
        private async Task<ICollection<string?>> GetMostViewedCategoriesAsync(Guid userId)
        {
            var posts = await _visitedPostQueryRepository.GetVisitedPostsByUserIdAsync(userId);
            var categories = posts
                .GroupBy(x => x.Post.SubCategory.CategoryId)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .ToList();
            var categoriesName = new List<string?>();
            foreach (var categoryGuid in categories) {
                categoriesName.Add((await _categoryQueryRepository.GetCategoryByIdAsync(categoryGuid)).NameEN);
            }

            return categoriesName;

        }
        private async Task<ICollection<string?>> GetMostViewedCitiesAsync(Guid userId)
        {
            var posts = await _visitedPostQueryRepository.GetVisitedPostsByUserIdAsync(userId);
            var cities = posts
                .GroupBy(p => p.Post.City)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key?.ToString())
                .ToList();
            return cities;
        }
    }
}
