using Application.Queries.Users.Chat.GetChatMesages;
using Application.Queries.Users.Chat.GetMyChats;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Users;

[Route("user/chats")]
[Authorize(Policy = "User")]
public class ChatsController : BaseController
{
    [HttpGet]
    [Route("my-chats")]
    public async Task<IActionResult> GetMyChats()
        => HandleResponse(await Mediator.Send(new GetMyChatsQuery()));
    

    [HttpPost]
    [Route("{id:guid}/messages")]
    public async Task<IActionResult> GetChatMessages([FromRoute] Guid id, [FromBody] GetChatMessagesQuery query)
    {
        query.ChatId = id;
        return HandleResponse(await Mediator.Send(query));
    }
}