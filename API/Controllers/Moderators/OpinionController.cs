using Application.Commands.Moderators.Opinions.DeleteOpinion;
using Application.Commands.Moderators.Opinions.SetOpinionHidden;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("moderator/opinions")]
[Authorize(Policy = "Moderator")]
public class ModeratorOpinionController : BaseController
{
    [HttpDelete]
    [Route("delete/{opinionId:guid}")]
    public async Task<IActionResult> DeleteOpinion([FromRoute] Guid opinionId)
    {
        return HandleResponse(await Mediator.Send(new DeleteOpinionCommand { OpinionId = opinionId }));
    }

    [HttpPatch]
    [Route("set-hidden")]
    public async Task<IActionResult> SetHiddenOpinion(SetOpinionHiddenCommand command)
    {
        return HandleResponse(await Mediator.Send(command));
    }
}