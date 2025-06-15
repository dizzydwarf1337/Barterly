using API.Core.ApiResponse;
using Application.DTOs.Posts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Posts.Queries.GetPostImages
{
    public class GetPostImagesCommand : IRequest<ApiResponse<PostImagesDto>>
    {
        public required string PostId { get; set; }
    }
}
