using Application.Commands.Admins.Reports.DeleteReport;
using Application.Commands.Admins.Reports.ReviewReport;
using Application.Queries.Admins.Reports.GetReportsFiltredPaginated;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("admin/reports")]
[Authorize(Policy = "Admin")]
public class AdminReportController : BaseController
{
    [HttpDelete]
    [Route("delete/{reportId:guid}")]
    public async Task<IActionResult> DeleteReport([FromRoute] Guid reportId)
    {
        return HandleResponse(await Mediator.Send(new DeleteReportCommand { ReportId = reportId }));
    }

    [HttpPatch]
    [Route("review")]
    public async Task<IActionResult> ReviewReport(ReviewReportCommand command)
    {
        return HandleResponse(await Mediator.Send(command));
    }

    [HttpPost]
    public async Task<IActionResult> GetPostFiltredPaginated(GetReportFiltredPaginatedQuery query)
    {
        return HandleResponse(await Mediator.Send(query));
    }
}