using Application.Commands.Admins.Opinions.DeleteOpinion;
using Application.Commands.Admins.Opinions.SetOpinionHidden;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("admin/opinions")]
[Authorize(Policy = "Admin")]
public class AdminOpinionController : BaseController
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