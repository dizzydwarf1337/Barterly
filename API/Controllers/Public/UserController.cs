using Application.Queries.Public.Users.GetUserById;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Public;

[Route("public/user")]
public class UserController : BaseController
{
    [HttpGet]
    [Route("post-owner/{id:guid}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        return HandleResponse(await Mediator.Send(new GetUserByIdQuery(){Id = id}));
    }
}