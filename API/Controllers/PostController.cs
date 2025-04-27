using API.Core.ApiResponse;
using Application.DTOs.Posts;
using Application.Features.Posts.Commands.CreatePost;
using Application.Features.Posts.Queries.GetPostById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class PostController : BaseController
    {

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreatePost([FromForm]CreatePostDto post)
        {
            return HandleResponse(await Mediator.Send(new CreatePostCommand { post = post }));
        }

        [HttpGet("{postId}")]
        public async Task<IActionResult> GetPostById([FromRoute] string postId)
        {
            return HandleResponse(await Mediator.Send(new GetPostByIdQuery { Id = postId }));
        }
    }
}
