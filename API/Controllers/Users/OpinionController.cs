using Application.Commands.Users.Opinions.CreateOpinion;
using Application.Commands.Users.Opinions.DeleteOpinion;
using Application.Commands.Users.Opinions.EditOpinion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("user/opinions")]
[Authorize(Policy = "User")]
public class UserOpinionController : BaseController
{
    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateOpinion(CreateOpinionCommand command)
    {
        return HandleResponse(await Mediator.Send(command));
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateOpinion(EditOpinionCommand command)
    {
        return HandleResponse(await Mediator.Send(command));
    }

    [HttpDelete]
    [Route("delete/{id:guid}")]
    public async Task<IActionResult> DeleteOpinion([FromRoute] Guid id)
    {
        return HandleResponse(await Mediator.Send(new DeleteOpinionCommand { OpinionId = id }));
    }
}