using Application.Commands.Users.User.UpdateProfile;
using Application.Queries.Public.Users.GetUserData;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Users;
[Route("user/authorized")]
public class UserController : BaseController
{
    [HttpGet]
    [Route("{userId:guid}")]
    public async Task<IActionResult> GetUserData([FromRoute] Guid userId)
        => HandleResponse(await Mediator.Send(new GetUserDataQuery
        {
            UserId = userId
        }));

    [HttpPost]
    [Route("update-profile")]
    public async Task<IActionResult> UpdateMyData([FromForm] UpdateProfileCommand command)
    => HandleResponse(await Mediator.Send(command));
    
}