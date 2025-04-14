using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Queries.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class RecommendationService : IRecommendationService
    {
        private readonly IUserActivityService _userActivityService;
        private readonly IPostQueryRepository _postQueryRepository;
        private readonly IMapper _mapper;
        
        public RecommendationService(IUserActivityService userActivityService,IPostQueryRepository postQueryRepository, IMapper mapper)
        {
            _userActivityService = userActivityService;
            _postQueryRepository = postQueryRepository;
            _mapper = mapper;
        }

        public async Task<ICollection<PostDto>> GetFeed(int page, Guid? userId = null)
        {
            
            ICollection<PostDto> posts = new List<PostDto>();
            ICollection<PostDto> promotedPosts = new List<PostDto>();
            if (userId==null)
            {
                posts = await GetPopularPosts(7, page);
                promotedPosts = await GetPromotedPosts(3);
            }
            else
            {
                var userActivity = await _userActivityService.GetUserActivityByUserId(userId.Value);
                posts = _mapper.Map<ICollection<PostDto>>(await _postQueryRepository.GetFeed(userActivity.MostViewedCategories, userActivity.MostViewedCities, 7, page));
                promotedPosts = await GetPromotedPosts(3, userActivity.MostViewedCategories, userActivity.MostViewedCities);
            }
            return ShufflePosts(posts,promotedPosts);
        }

        public Task<ICollection<PostDto>> GetPopularPosts(int pageCount, int Page)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<PostDto>> GetRecommendationPostsByUser(int count, Guid userId)
        {
            throw new NotImplementedException();
        }
        public async Task<ICollection<PostDto>> GetPromotedPosts(int count, string? categories = null, string? cities = null)
        {

            ICollection<Post> posts = await _postQueryRepository.GetPromotedPosts(count, categories, cities);
            return _mapper.Map<ICollection<PostDto>>(posts);
        }


        private ICollection<PostDto> ShufflePosts(ICollection<PostDto> posts, ICollection<PostDto> promotedPosts)
        {
            if (promotedPosts.Count == 0)
                return posts;

            int proportion = Math.Max(1, posts.Count / promotedPosts.Count);
            List<PostDto> shuffledPosts = new List<PostDto>();
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
    }
}
