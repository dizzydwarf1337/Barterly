using API.Core.ApiResponse;
using Application.DTOs.Posts;
using Application.Interfaces;
using Domain.Exceptions.BusinessExceptions;
using Domain.Exceptions.DataExceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Posts.Commands.CreatePost
{
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, ApiResponse<PostDto>>
    {
        private readonly IPostService _postService;

        public CreatePostCommandHandler(IPostService postService)
        {
            _postService = postService;
        }

        public async Task<ApiResponse<PostDto>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var post = await _postService.CreatePost(request.post);
                return ApiResponse<PostDto>.Success(post,201);
            }
            catch (InvalidDataProvidedException ex)
            {   
                return ApiResponse<PostDto>.Failure(ex.Message);
            } 
            catch (EntityCreatingException ex)
            {
                return ApiResponse<PostDto>.Failure(ex.Message,409);
            }
            catch (Exception ex) 
            {
                Console.WriteLine(request.post.Title + " " + request.post.FullDescription + " " + request.post.City);
                Console.WriteLine("Error: " + ex.Message);
                return ApiResponse<PostDto>.Failure("Error while creating new post");
            }
        }
    }
}
