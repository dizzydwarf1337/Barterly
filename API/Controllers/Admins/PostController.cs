using Application.Commands.Admins.Posts.ApprovePost;
using Application.Commands.Admins.Posts.DeletePost;
using Application.Commands.Admins.Posts.RejectPost;
using Application.Queries.Admins.Posts.GetPostById;
using Application.Queries.Admins.Posts.GetPostFiltredPaginated;
using Application.Queries.Public.Posts.GetPostImages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Admins;

[Route("admin/posts")]
[Authorize(Policy = "Admin")]
public class AdminPostController : BaseController
{
    [HttpGet]
    [Route("get/{postId:guid}")]
    public async Task<IActionResult> GetPostById([FromRoute] Guid postId)
    {
        return HandleResponse(await Mediator.Send(new GetPostByIdQuery { PostId = postId }));
    }

    [HttpDelete]
    [Route("delete/{postId:guid}")]
    public async Task<IActionResult> DeletePost([FromRoute] Guid postId)
    {
        return HandleResponse(await Mediator.Send(new DeletePostCommand { PostId = postId }));
    }

    [HttpPost]
    [Route("approve-post")]
    public async Task<IActionResult> ApprovePost([FromBody] ApprovePostCommand command)
    {
        return HandleResponse(await Mediator.Send(command));
    }


    [HttpPost]
    [Route("reject-post")]
    public async Task<IActionResult> RejectPost([FromBody] RejectPostCommand command)
    {
        return HandleResponse(await Mediator.Send(command));
    }

    [HttpGet]
    [Route("images/{postId:guid}")]
    public async Task<IActionResult> GetPostImages([FromRoute] Guid postId)
    {
        return HandleResponse(await Mediator.Send(new GetPostImagesCommand { PostId = postId }));
    }
    
    [HttpPost]
    public async Task<IActionResult> GetPosts(GetPostsQuery query)
        => HandleResponse(await Mediator.Send(query));
}