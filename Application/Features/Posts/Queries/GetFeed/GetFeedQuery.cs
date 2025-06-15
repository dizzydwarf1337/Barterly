using API.Core.ApiResponse;
using Application.DTOs.Posts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Posts.Queries.GetFeed
{
    public class GetFeedQuery : IRequest<ApiResponse<ICollection<PostPreviewDto>>>
    {
        public required int page { get; set; }
        public string? userId { get; set; }
    }
}
