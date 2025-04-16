using Application.Interfaces;
using Domain.Entities.Users;
using Domain.Interfaces.Commands.Post;
using Domain.Interfaces.Commands.User;
using Domain.Interfaces.Queries.Post;
using Domain.Interfaces.Queries.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserActivityService : IUserActivityService
    {
        private readonly IUserActivityCommandRepository _userActivityCommandRepository;
        private readonly IUserActivityQueryRepository _userActivityQueryRepository;
        private readonly IVisitingPostService _visitingPostService;

        public UserActivityService(
            IUserActivityCommandRepository userActivityCommandRepository,
            IUserActivityQueryRepository userActivityQueryRepository, 
            IVisitingPostService visitingPostService
            )
        {
            _userActivityCommandRepository = userActivityCommandRepository;
            _userActivityQueryRepository = userActivityQueryRepository;
            _visitingPostService = visitingPostService;
        }
        public async Task DeleteUserActivity(Guid userId)
        {
             await _userActivityCommandRepository.DeleteUserActivitySummary(userId);
        }

        public async Task<UserActivitySummary> GetUserActivityByUserId(Guid userId)
        {
            return await _userActivityQueryRepository.GetUserActivityByUserIdAsync(userId);   
        }

        public async Task<UserActivitySummary> SummarizeUserActivity(Guid userId)
        {
            var userActivity = await GetUserActivityByUserId(userId);
            userActivity.MostViewedCities = String.Join(",",await GetMostViewedCitiesAsync(userId));
            userActivity.MostViewedCategories = String.Join(",",await GetMostViewedCategoriesAsync(userId));
            await _userActivityCommandRepository.UpdateUserActivitySummary(userActivity);
            return userActivity;
        }

        public async Task<UserActivitySummary> CreateUserActivity(Guid userId)
        {
            UserActivitySummary? userActivity;
            try {
                userActivity = await _userActivityQueryRepository.GetUserActivityByUserIdAsync(userId);
            }
            catch
            {
                userActivity = null;
            }
            if (userActivity!= null)
            {
                return await SummarizeUserActivity(userId);
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
            var posts = await _visitingPostService.GetVisitedPostsByUserIdAsync(userId);
            var categories = posts
                .GroupBy(x => x.Post.SubCategory.Category)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key.ToString())
                .ToList();
            return categories;

        }
        private async Task<ICollection<string?>> GetMostViewedCitiesAsync(Guid userId)
        {
            var posts = await _visitingPostService.GetVisitedPostsByUserIdAsync(userId);
            var cities = posts
                .GroupBy(p=>p.Post.City)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key?.ToString())
                .ToList();
            return cities;
        }
    }
}
