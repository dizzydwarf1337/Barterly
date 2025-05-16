using API.Core.ApiResponse;
using Application.DTOs.Posts;
using Application.Features.Posts.Commands.ApprovePost;
using Application.Features.Posts.Commands.CreatePost;
using Application.Features.Posts.Commands.DeletePost;
using Application.Features.Posts.Commands.RejectPost;
using Application.Features.Posts.Commands.UpdatePost;
using Application.Features.Posts.Commands.UpdatePostImages;
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
        public async Task<IActionResult> CreatePost([FromForm] CreatePostDto post)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            Console.WriteLine(userId);
            return HandleResponse(await Mediator.Send(new CreatePostCommand { post = post }));
        }

        [HttpGet("{postId}")]
        public async Task<IActionResult> GetPostById([FromRoute] string postId)
        {
            return HandleResponse(await Mediator.Send(new GetPostByIdQuery { Id = postId }));
        }
        [HttpPut("update")]
        [Authorize]
        public async Task<IActionResult> UpdatePost([FromBody] EditPostDto post)
        {
            return HandleResponse(await Mediator.Send(new UpdatePostCommand { post = post }));
        }
        [HttpPatch("updateImages")]
        [Authorize]
        public async Task<IActionResult> UploadImages([FromForm] ImagesDto imagesDto)
        {
            return HandleResponse(await Mediator.Send(new UpdatePostImagesCommand { ImagesDto = imagesDto }));
        }
        [HttpDelete("{postId}")]
        [Authorize]
        public async Task<IActionResult> DeletePost([FromRoute] string postId)
        {
            return HandleResponse(await Mediator.Send(new DeletePostCommand { PostId = Guid.Parse(postId)}));
        }
        [HttpPost("approve")]
        [Authorize(Policy = "Admin")]
        [Authorize(Policy = "Moderator")]
        public async Task<IActionResult> ApprovePost([FromBody] ApprovePostDto approvePostDto)
        {
            return HandleResponse(await Mediator.Send(new ApprovePostCommand { ApprovePostDto = approvePostDto }));
        }
        [HttpPost("reject")]
        [Authorize(Policy = "Admin")]
        [Authorize(Policy = "Moderator")]
        public async Task<IActionResult> RejectPost([FromBody] RejectPostDto rejectPostDto)
        {
            return HandleResponse(await Mediator.Send(new RejectPostCommand { RejectPostDto = rejectPostDto }));
        }
    }
}
