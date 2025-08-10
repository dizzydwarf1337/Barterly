using Application.Commands.Admins.Users.CreateUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("admin/users")]
[Authorize(Policy = "Admin")]
public class AdminUserController : BaseController
{
    [HttpPost]
    [Route("create-user")]
    public async Task<IActionResult> CreateUser(CreateUserCommand command)
    {
        return HandleResponse(await Mediator.Send(command));
    }
}