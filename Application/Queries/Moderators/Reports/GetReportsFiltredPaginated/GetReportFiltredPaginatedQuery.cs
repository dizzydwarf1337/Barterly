using Application.Core.MediatR.Requests;
using Domain.Enums.Common;

namespace Application.Queries.Moderators.Reports.GetReportsFiltredPaginated;

public class GetReportFiltredPaginatedQuery : ModeratorRequest<GetReportFiltredPaginatedQuery.Result>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public Guid? AuthorId { get; set; }
    public Guid? SubjectId { get; set; }
    public ReportStatusType? Status { get; set; }

    public record Report(Guid Id, Guid AuthorId, Guid SubjectId, string Message, DateTime CreatedAt);

    public record Result(IEnumerable<Report> Reports);
}