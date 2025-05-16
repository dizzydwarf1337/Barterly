using API.Core.ApiResponse;
using Application.DTOs.Posts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Posts.Commands.UpdatePostImages
{
    public class UpdatePostImagesCommand : IRequest<ApiResponse<PostDto>>
    {
        public ImagesDto ImagesDto { get; set; }
    }
}
