using Application.DTOs.General.Opinions;
using Application.Features.Opinions.Commands.CreateOpinion;
using Application.Features.Opinions.Commands.DeleteOpinion;
using Application.Features.Opinions.Commands.EditOpinion;
using Application.Features.Opinions.Commands.SetOpinionHidden;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    
    public class OpinionController : BaseController
    {
        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateOpinion([FromBody] CreateOpinionDto opinion)
        {
            return HandleResponse(await Mediator.Send(new CreateOpinionCommand { createOpinionDto = opinion }));
        }
        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateOpinion([FromBody] EditOpinionDto editOpinionDto)
        {
            return HandleResponse(await Mediator.Send(new EditOpinionCommand { EditOpinionDto = editOpinionDto }));
        }
        [Authorize]
        [HttpDelete("{opinionId}")]
        public async Task<IActionResult> DeleteOpinion([FromRoute] string opinionId)
        {
            return HandleResponse(await Mediator.Send(new DeleteOpinionCommand { OpinionId = opinionId }));
        }
        [Authorize(Roles ="Admin,Moderator")]
        [HttpPatch("setHidden")]
        public async Task<IActionResult> SetHiddenOpinion([FromBody] HideOpinionDto hideOpinionDto)
        {
            return HandleResponse(await Mediator.Send(new SetOpinionHiddenCommand { HideOpinionDto = hideOpinionDto }));
        }
    }
}
