using API.Core.ApiResponse;
using Application.DTOs.Posts;
using Application.Interfaces;
using Domain.Exceptions.BusinessExceptions;
using MediatR;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Posts.Queries.GetFeed
{
    public class GetFeedQueryHanlder : IRequestHandler<GetFeedQuery, ApiResponse<ICollection<PostPreviewDto>>>
    {
        private readonly IRecommendationService _recommendationService;

        public GetFeedQueryHanlder(IRecommendationService recommendationService)
        {
            _recommendationService = recommendationService;
        }

        public async Task<ApiResponse<ICollection<PostPreviewDto>>> Handle(GetFeedQuery request, CancellationToken cancellationToken)
        {

            if (request.userId == null)
            {
                return ApiResponse<ICollection<PostPreviewDto>>.Success(await _recommendationService.GetFeed(request.page));
            }
            else
            {
                return ApiResponse<ICollection<PostPreviewDto>>.Success(await _recommendationService.GetFeed(request.page,Guid.Parse(request.userId)));
            }

        }
    }
}
