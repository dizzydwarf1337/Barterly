using Application.DTOs.Reports;
using Application.Features.Reports.Commands.CreateReport;
using Application.Features.Reports.Commands.DeleteReport;
using Application.Features.Reports.Commands.ReviewReport;
using Application.Features.Reports.Queries.GetReportsFiltredPaginated;
using Domain.Enums.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    public class ReportController : BaseController
    {
        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateReport(CreateReportDto createReportDto)
        {
            return HandleResponse(await Mediator.Send(new CreateReportCommand { createReportDto = createReportDto }));
        }


        [Authorize(Roles ="Admin,Moderator")]
        [HttpDelete("{reportId}")]
        public async Task<IActionResult> DeleteReport([FromRoute] string reportId)
        {
            return HandleResponse(await Mediator.Send(new DeleteReportCommand { ReportId = reportId }));
        }

        [Authorize(Roles ="Admin,Moderator")]
        [HttpPatch("review")]
        public async Task<IActionResult> ReviewReport(ReviewReportDto reviewReportDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return HandleResponse(await Mediator.Send(new ReviewReportCommand { ReviewDto = reviewReportDto, AuthoritiesId = userId!}));
        }
        [Authorize(Roles ="Admin,Moderator")]
        [HttpGet]
        public async Task<IActionResult> GetPostFiltredPaginated([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] Guid? authorId = null, [FromQuery] Guid? subjectId = null, [FromQuery] ReportStatusType? status = null)
        {
            return HandleResponse(await Mediator.Send(new GetReportFiltredPaginatedQuery { Page = page, PageSize = pageSize, AuthorId = authorId, SubjectId = subjectId, Status = status }));
        }
    }
}
