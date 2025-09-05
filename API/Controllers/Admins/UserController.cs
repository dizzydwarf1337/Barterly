using Application.Commands.Admins.Users.CreateUser;
using Application.Commands.Admins.Users.DeleteUser;
using Application.Commands.Admins.Users.UpdateUserSettings;
using Application.Queries.Admins.Users.GetUserById;
using Application.Queries.Admins.Users.GetUsers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Admins;

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

    [HttpPost]
    public async Task<IActionResult> GetUsers(GetUsersQuery query)
    {
        return HandleResponse(await Mediator.Send(query));
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        return HandleResponse(await Mediator.Send(new DeleteUserCommand()
        {
            Id = id
        }));
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        return HandleResponse(await Mediator.Send(new GetUserByIdQuery() { Id = id }));
    }

    [HttpPost]
    [Route("settings")]
    public async Task<IActionResult> UpdateUserSettings(UpdateUserSettingsCommand command)
    {
        return HandleResponse(await Mediator.Send(command));
    }
}