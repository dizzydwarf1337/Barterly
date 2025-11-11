using Application.Commands.Users.Notifications.ReadNotification;
using Application.Queries.Users.Notifications.MyNotifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Users;

[Route("notifications")]
public class NotificationsController : BaseController
{
    [HttpGet]
    [Route("my-notifications")]
    public async Task<IActionResult> GetMyNotifications()
    {
        return HandleResponse(await Mediator.Send(new GetMyNotificationsQuery()));
    }

    [HttpPost]
    [Route("read/{id:guid}")]
    public async Task<IActionResult> ReadNotifications([FromRoute] Guid id)
    {
        var command = new ReadNotificationCommand
        {
            NotificationId = id
        };
        return HandleResponse(await Mediator.Send(command));
    }
}