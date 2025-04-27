using Application.DTOs.Posts;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.Posts;
using Domain.Interfaces.Queries.Post;

namespace Application.Services
{
    public class RecommendationService : IRecommendationService
    {
        private readonly IUserActivityService _userActivityService;
        private readonly IPostQueryRepository _postQueryRepository;
        private readonly IMapper _mapper;
        private const int DefaultPageSize = 7;
        private const int PromotedPostsPerPage = 3;
        public RecommendationService(IUserActivityService userActivityService, IPostQueryRepository postQueryRepository, IMapper mapper)
        {
            _userActivityService = userActivityService;
            _postQueryRepository = postQueryRepository;
            _mapper = mapper;
        }

        public async Task<ICollection<PostPreviewDto>> GetFeed(int page, Guid? userId = null)
        {

            ICollection<PostPreviewDto> posts = new List<PostPreviewDto>();
            ICollection<PostPreviewDto> promotedPosts = new List<PostPreviewDto>();
            if (userId == null)
            {
                posts = await GetPopularPosts(DefaultPageSize);
                promotedPosts = await GetPromotedPosts(PromotedPostsPerPage);
            }
            else
            {
                var userActivity = await _userActivityService.GetUserActivityByUserId(userId.Value);
                var mostViewedCategories = userActivity.MostViewedCategories?.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).ToList() ?? new List<string>();
                var mostViewedCitites = userActivity.MostViewedCities?.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).ToList() ?? new List<string>();
                posts = _mapper.Map<ICollection<PostPreviewDto>>(await _postQueryRepository.GetFeed(mostViewedCategories, mostViewedCitites, DefaultPageSize, page));
                promotedPosts = await GetPromotedPosts(PromotedPostsPerPage, mostViewedCategories,mostViewedCitites);
            }
            return ShufflePosts(posts, promotedPosts);
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


        private ICollection<PostPreviewDto> ShufflePosts(ICollection<PostPreviewDto> posts, ICollection<PostPreviewDto> promotedPosts)
        {
            if (promotedPosts.Count == 0)
                return posts;

            int proportion = Math.Max(1, posts.Count / promotedPosts.Count);
            List<PostPreviewDto> shuffledPosts = new List<PostPreviewDto>();
            using var postEnumerator = posts.GetEnumerator();
            using var promotedEnumerator = promotedPosts.GetEnumerator();

            while (postEnumerator.MoveNext())
            {
                shuffledPosts.Add(postEnumerator.Current);

                if (shuffledPosts.Count % proportion == 0 && promotedEnumerator.MoveNext())
                {
                    shuffledPosts.Add(promotedEnumerator.Current);
                }
            }

            while (promotedEnumerator.MoveNext())
            {
                shuffledPosts.Add(promotedEnumerator.Current);
            }

            return shuffledPosts;
        }

        public async Task<ICollection<PostPreviewDto>> GetFiltredPosts(int? pageCount, int? page, Guid? subCategoryId = null, string? city = null, string? region = null)
        {
            var posts = await _postQueryRepository.GetFiltredPostsAsync(pageCount, page, subCategoryId, city, region);
            return _mapper.Map<ICollection<PostPreviewDto>>(posts);
        }
    }
}
