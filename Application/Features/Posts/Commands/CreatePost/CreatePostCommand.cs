using API.Core.ApiResponse;
using Application.DTOs.Posts;
using Application.Interfaces.CommandInterfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Posts.Commands.CreatePost
{
    public class CreatePostCommand : IRequest<ApiResponse<PostDto>>, IHasOwner
    {
        public required CreatePostDto Post { get; set; }
        public string OwnerId => Post.OwnerId;
    }
}
