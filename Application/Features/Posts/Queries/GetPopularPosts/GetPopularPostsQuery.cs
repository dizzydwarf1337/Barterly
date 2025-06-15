using API.Core.ApiResponse;
using Application.DTOs.Posts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Posts.Queries.GetPopularPosts
{
    public class GetPopularPostsQuery : IRequest<ApiResponse<ICollection<PostPreviewDto>>>
    {
        public int Count { get; set; }
        public string? City { get; set; }
    }
    
}
