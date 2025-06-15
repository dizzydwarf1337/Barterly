using API.Core.ApiResponse;
using Application.DTOs.General.Opinions;
using Application.DTOs.Posts;
using Application.Features.Posts.Commands.ApprovePost;
using Application.Features.Posts.Commands.CreatePost;
using Application.Features.Posts.Commands.DeletePost;
using Application.Features.Posts.Commands.RejectPost;
using Application.Features.Posts.Commands.UpdatePost;
using Application.Features.Posts.Commands.UpdatePostImages;
using Application.Features.Posts.Queries.GetPostById;
using Application.Features.Posts.Queries.GetPostImages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace API.Controllers
{
    public class PostController : BaseController
    {

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreatePost([FromForm] CreatePostDto post)
        {
            return HandleResponse(await Mediator.Send(new CreatePostCommand { Post = post }));
        }

        [HttpGet("{postId}")]
        public async Task<IActionResult> GetPostById([FromRoute] string postId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return HandleResponse(await Mediator.Send(new GetPostByIdQuery { postId = postId, userId = userId }));
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
            return HandleResponse(await Mediator.Send(new DeletePostCommand { PostId = postId}));
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
        [HttpGet("images/{postId}")]
        public async Task<IActionResult> GetPostImages([FromRoute] string postId)
        {
            return HandleResponse(await Mediator.Send(new GetPostImagesCommand { PostId = postId }));
        }


    }
}
