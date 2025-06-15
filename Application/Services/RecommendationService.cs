using Application.DTOs.Posts;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.Posts;
using Domain.Interfaces.Queries.Post;
using Domain.Interfaces.Queries.User;

namespace Application.Services
{
    public class RecommendationService : IRecommendationService
    {
        private readonly IUserActivityQueryRepository _userActivityQueryRepository;
        private readonly IPostQueryRepository _postQueryRepository;
        private readonly IMapper _mapper;
        private const int DefaultPageSize = 10;
        private const int DefaultPostsCount = 7;
        private const int DefaultPromotedPostsCount = 3;
        public RecommendationService(IUserActivityQueryRepository userActivityQueryRepository, IPostQueryRepository postQueryRepository, IMapper mapper)
        {
            _userActivityQueryRepository = userActivityQueryRepository;
            _postQueryRepository = postQueryRepository;
            _mapper = mapper;
        }

        public async Task<ICollection<PostPreviewDto>> GetFeed(int page, Guid? userId = null)
        {

            ICollection<PostPreviewDto> posts = new HashSet<PostPreviewDto>();
            ICollection<PostPreviewDto> promotedPosts = new List<PostPreviewDto>();
            if (userId == null)
            {
                posts = await GetPopularPosts(DefaultPostsCount);
                promotedPosts = await GetPromotedPosts(DefaultPromotedPostsCount);
            }
            else
            {
                var userActivity = await _userActivityQueryRepository.GetUserActivityByUserIdAsync(userId.Value);
                var mostViewedCategories = userActivity.MostViewedCategories?.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).ToList() ?? new List<string>();
                var mostViewedCitites = userActivity.MostViewedCities?.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).ToList() ?? new List<string>();
                posts = _mapper.Map<ICollection<PostPreviewDto>>(await _postQueryRepository.GetFeed(mostViewedCategories, mostViewedCitites, DefaultPostsCount, page));
                promotedPosts = await GetPromotedPosts(DefaultPromotedPostsCount, mostViewedCategories,mostViewedCitites);
            }
            foreach(var post in promotedPosts)
            {
                posts.Add(post);
            }
            return posts.OrderBy(x => Guid.NewGuid()).ToList();
        }

        public async Task<ICollection<PostPreviewDto>> GetPopularPosts(int count, string? city = null)
        {
            var posts = await _postQueryRepository.GetPopularPosts(count, city);
            return _mapper.Map<ICollection<PostPreviewDto>>(posts);
        }

        public async Task<ICollection<PostPreviewDto>> GetPromotedPosts(int count, List<string>? categories = null, List<string>? cities = null)
        {

            ICollection<Post> posts = await _postQueryRepository.GetPromotedPosts(count, categories, cities);
            return _mapper.Map<ICollection<PostPreviewDto>>(posts);
        }
        public async Task<ICollection<PostPreviewDto>> GetFiltredPosts(int? pageCount, int? page, Guid? subCategoryId = null, string? city = null, string? region = null)
        {
            var posts = await _postQueryRepository.GetFiltredPostsAsync(pageCount, page, subCategoryId, city, region);
            return _mapper.Map<ICollection<PostPreviewDto>>(posts);
        }
    }
}
