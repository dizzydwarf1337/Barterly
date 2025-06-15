using API.Core.ApiResponse;
using Application.DTOs.Posts;
using Application.Interfaces.CommandInterfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Posts.Commands.UpdatePost
{
    public class UpdatePostCommand : IRequest<ApiResponse<PostDto>>, IHasOwner, IPostOwner
    {
        public required EditPostDto post { get; set; }

        public string OwnerId => post.OwnerId;

        public string PostId => post.Id;
    }
}
