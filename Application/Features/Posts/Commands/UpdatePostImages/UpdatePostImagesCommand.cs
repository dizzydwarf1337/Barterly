using API.Core.ApiResponse;
using Application.DTOs.Posts;
using Application.Interfaces.CommandInterfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Posts.Commands.UpdatePostImages
{
    public class UpdatePostImagesCommand : IRequest<ApiResponse<Unit>>, IHasOwner, IPostOwner 
    {
        public required ImagesDto ImagesDto { get; set; }

        public string OwnerId => ImagesDto.UserId;

        public string PostId => ImagesDto.PostId;
    }
}
