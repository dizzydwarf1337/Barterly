using API.Core.ApiResponse;
using Application.DTOs.Posts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Posts.Queries.GetPostById
{
    public class GetPostByIdQuery : IRequest<ApiResponse<PostDto>>
    {
        public string Id { get; set; }
    }
}
