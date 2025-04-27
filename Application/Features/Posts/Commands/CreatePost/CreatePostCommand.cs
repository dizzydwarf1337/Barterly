using API.Core.ApiResponse;
using Application.DTOs.Posts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Posts.Commands.CreatePost
{
    public class CreatePostCommand : IRequest<ApiResponse<PostDto>>
    {
        public CreatePostDto post { get; set; }
    }
}
