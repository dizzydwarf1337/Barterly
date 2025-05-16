using API.Core.ApiResponse;
using Application.DTOs.Posts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Posts.Commands.RejectPost
{
    public class RejectPostCommand : IRequest<ApiResponse<Unit>>
    {
        public RejectPostDto RejectPostDto { get; set; }
    }
}
