using Application.Commands.Users.Posts.AddFavourite;
using Application.Commands.Users.Posts.BuyPromotion;
using Application.Commands.Users.Posts.CreatePost;
using Application.Commands.Users.Posts.DeletePost;
using Application.Commands.Users.Posts.UpdatePost;
using Application.Commands.Users.Posts.UpdatePostImages;
using Application.Commands.Users.Posts.UpdatePostVisibility;
using Application.Queries.Public.Posts.GetFeed;
using Application.Queries.Public.Posts.GetPostImages;
using Application.Queries.Users.Posts.GetFavPosts;
using Application.Queries.Users.Posts.GetMyPosts;
using Application.Queries.Users.Posts.GetPopularPosts;
using Application.Queries.Users.Posts.GetPostById;
using Application.Queries.Users.Posts.GetPostPreviewById;
using Application.Queries.Users.Posts.GetPostsFiltredPaginated;
using Domain.Enums.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Users;

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
        return HandleResponse(await Mediator.Send(new GetPopularPostsQuery() { Count = count }));
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreatePost([FromForm]CreatePostCommand command)
    {
        if (Request.Form.TryGetValue("currency", out var currencyValue) &&
            int.TryParse(currencyValue, out int currencyInt) &&
            Enum.IsDefined(typeof(PostCurrency), currencyInt))
        {
            command.Currency = (PostCurrency)currencyInt;
        }
        else
        {
            return BadRequest("Invalid currency value");
        }
        
        if (Request.Form.TryGetValue("postPriceType", out var postPriceTypeValue) &&
            int.TryParse(postPriceTypeValue, out int postPriceTypeInt) &&
            Enum.IsDefined(typeof(PostPriceType), postPriceTypeInt))
        {
            command.PostPriceType = (PostPriceType)postPriceTypeInt;
        }

        if (Request.Form.TryGetValue("workload", out var workloadValue) &&
            int.TryParse(workloadValue, out int workloadInt) &&
            Enum.IsDefined(typeof(WorkloadType), workloadInt))
        {
            command.Workload = (WorkloadType)workloadInt;
        }
        
        if (Request.Form.TryGetValue("workLocation", out var workLocationValue) &&
            int.TryParse(workLocationValue, out int workLocationInt) &&
            Enum.IsDefined(typeof(WorkLocationType), workLocationInt))
        {
            command.WorkLocation = (WorkLocationType)workLocationInt;
        }
        
        if (Request.Form.TryGetValue("rentObjectType", out var rentObjectTypeValue) &&
            int.TryParse(workLocationValue, out int rentObjectTypeInt) &&
            Enum.IsDefined(typeof(RentObjectType), rentObjectTypeInt))
        {
            command.RentObjectType = (RentObjectType)rentObjectTypeInt;
        }
        
        
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

    [HttpPut]
    [Route("fav-post/{id:guid}")]
    public async Task<IActionResult> FavPost([FromRoute] Guid id)
        => HandleResponse(await Mediator.Send(new AddFavouriteCommand() { Id = id }));

    [HttpPost]
    [Route("fav-posts")]
    public async Task<IActionResult> GetFavouritePosts(GetFavPostsQuery command)
        => HandleResponse(await Mediator.Send(command));

    [HttpPost]
    [Route("feed")]
    public async Task<IActionResult> GetFeed(GetFeedQuery query)
    {
        return HandleResponse(await Mediator.Send(query));
    }

    [HttpGet]
    [Route("my-posts")]
    public async Task<IActionResult> GetMyPosts()
        => HandleResponse(await Mediator.Send(new GetMyPostsQuery()));

    [HttpPut]
    [Route("hide/{id:guid}")]
    public async Task<IActionResult> HidePost([FromRoute] Guid id)
        => HandleResponse(await Mediator.Send(new UpdatePostVisibilityCommand
        {
            PostId = id
        }));

    [HttpPut]
    [Route("buy-promotion")]
    public async Task<IActionResult> BuyPromotion(BuyPromotionCommand command)
        => HandleResponse(await Mediator.Send(command));

    [HttpGet]
    [Route("preview/{id:guid}")]
    public async Task<IActionResult> GetPostPreview([FromRoute] Guid id)
        => HandleResponse(await Mediator.Send(new GetPostPreviewByIdQuery
        {
            PostId = id
        }));
}