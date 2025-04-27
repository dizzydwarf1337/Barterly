using API.Core.ApiResponse;
using Application.DTOs.Posts;
using Application.Interfaces;
using Domain.Exceptions.BusinessExceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Posts.Queries.GetPostById
{
    public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, ApiResponse<PostDto>>
    {
        private readonly IPostService _postService;

        public GetPostByIdQueryHandler(IPostService postService)
        {
            _postService = postService;
        }

        public async Task<ApiResponse<PostDto>> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var post = await _postService.GetPostById(Guid.Parse(request.Id));
                return ApiResponse<PostDto>.Success(post);
            }
            catch (EntityNotFoundException ex)
            {
                return ApiResponse<PostDto>.Failure(ex.Message, 404);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return ApiResponse<PostDto>.Failure("Errow while getting post by id");
            }
        }
    }
}
