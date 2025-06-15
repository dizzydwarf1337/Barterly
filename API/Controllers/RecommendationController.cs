using Application.Features.Posts.Queries.GetFeed;
using Application.Features.Posts.Queries.GetPopularPosts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    public class RecommendationController : BaseController
    {
        [HttpGet("popular")]
        public async Task<IActionResult> GetPopularPosts([FromQuery] int count, [FromQuery] string? city)
        {
            return HandleResponse(await Mediator.Send(new GetPopularPostsQuery { Count = count, City = city }));
        }
        [HttpGet("feed")]
        public async Task<IActionResult> GetFeed([FromQuery] int page)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return HandleResponse(await Mediator.Send(new GetFeedQuery { page=page,userId = userId }));
        }
    }
}
