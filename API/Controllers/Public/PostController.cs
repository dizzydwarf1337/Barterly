using Application.Queries.Public.Posts.GetFeed;
using Application.Queries.Public.Posts.GetPostById;
using Application.Queries.Public.Posts.GetPostImages;
using Application.Queries.Public.Posts.GetPostsFiltredPaginated;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Public;

[Route("public/posts")]
[AllowAnonymous]
public class PostController : BaseController
{
    [HttpGet]
    [Route("get/{postId:guid}")]
    public async Task<IActionResult> GetPostById([FromRoute] Guid postId)
    {
        return HandleResponse(await Mediator.Send(new GetPostByIdQuery { PostId = postId }));
    }

    [HttpGet]
    [Route("images/{postId:guid}")]
    public async Task<IActionResult> GetPostImages([FromRoute] Guid postId)
    {
        return HandleResponse(await Mediator.Send(new GetPostImagesCommand { PostId = postId }));
    }

    [HttpPost]
    [Route("feed")]
    public async Task<IActionResult> GetFeed(GetFeedQuery query)
    {
        return HandleResponse(await Mediator.Send(query));
    }

    [HttpPost]
    [Route("search")]
    public async Task<IActionResult> GetPosts([FromBody] GetPostsQuery query)
    {
        return HandleResponse(await Mediator.Send(query));
    }
}