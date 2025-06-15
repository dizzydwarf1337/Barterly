using API.Core.ApiResponse;
using Application.Interfaces.CommandInterfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Posts.Commands.DeletePost
{
    public class DeletePostCommand : IRequest<ApiResponse<Unit>>, IPostOwner
    {
        public required string PostId { get; set; }

    }
    
}
