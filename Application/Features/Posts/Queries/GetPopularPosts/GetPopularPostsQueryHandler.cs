using API.Core.ApiResponse;
using Application.DTOs.Posts;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Posts.Queries.GetPopularPosts
{
    public class GetPopularPostsQueryHandler : IRequestHandler<GetPopularPostsQuery, ApiResponse<ICollection<PostPreviewDto>>>
    {
        private readonly IRecommendationService recommendationService;

        public GetPopularPostsQueryHandler(IRecommendationService recommendationService)
        {
            this.recommendationService = recommendationService;
        }

        public async Task<ApiResponse<ICollection<PostPreviewDto>>> Handle(GetPopularPostsQuery request, CancellationToken cancellationToken)
        {

            return ApiResponse<ICollection<PostPreviewDto>>.Success(await recommendationService.GetPopularPosts(request.Count, request.City));
          
        }
    }

}
