using Application.Commands.Users.Reports.CreateReport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("user/reports")]
[Authorize(Policy = "User")]
public class UserReportController : BaseController
{
    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateReport(CreateReportCommand command)
    {
        return HandleResponse(await Mediator.Send(command));
    }
}