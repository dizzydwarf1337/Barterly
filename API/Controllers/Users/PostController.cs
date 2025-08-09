using Application.Commands.Users.Posts.CreatePost;
using Application.Commands.Users.Posts.DeletePost;
using Application.Commands.Users.Posts.UpdatePost;
using Application.Commands.Users.Posts.UpdatePostImages;
using Application.Queries.Public.Posts.GetFeed;
using Application.Queries.Public.Posts.GetPostById;
using Application.Queries.Public.Posts.GetPostImages;
using Application.Queries.Public.Posts.GetPostsFiltredPaginated;
using Application.Queries.Users.Posts.GetPopularPosts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Public;

[Route("user/posts")]
[Authorize(Policy = "User")]
public class UserPostController : BaseController
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

    [HttpGet]
    [Route("feed")]
    public async Task<IActionResult> GetFeed([FromQuery] int PageNumber, [FromQuery] int PageSize)
    {
        return HandleResponse(await Mediator.Send(new GetFeedQuery { PageNumber = PageNumber, PageSize = PageSize }));
    }

    [HttpPost]
    [Route("search")]
    public async Task<IActionResult> GetPosts([FromBody] GetPostsQuery query)
    {
        return HandleResponse(await Mediator.Send(query));
    }

    [HttpGet]
    [Route("popular")]
    public async Task<IActionResult> GetPopularPosts([FromQuery] int count)
    {
        return HandleResponse(await Mediator.Send(new GetPopularPostsQuery { Count = count }));
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostCommand command)
    {
        return HandleResponse(await Mediator.Send(command));
    }

    [HttpDelete]
    [Route("delete/{id:guid}")]
    public async Task<IActionResult> DeletePost([FromRoute] Guid id)
    {
        return HandleResponse(await Mediator.Send(new DeletePostCommand { PostId = id }));
    }

    [HttpPut]
    [Route("update-post")]
    public async Task<IActionResult> UpdatePost([FromBody] UpdatePostCommand command)
    {
        return HandleResponse(await Mediator.Send(command));
    }

    [HttpPut]
    [Route("update-images")]
    public async Task<IActionResult> UpdatePostImages([FromBody] UpdatePostImagesCommand command)
    {
        return HandleResponse(await Mediator.Send(command));
    }
}