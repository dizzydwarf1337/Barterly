using Application.Core.ApiResponse;
using AutoMapper;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Queries.Post;
using Domain.Interfaces.Queries.User;
using MediatR;

namespace Application.Queries.Admins.Reports.GetReportsFiltredPaginated;

public class GetReportFiltredPaginatedQueryHandler : IRequestHandler<GetReportFiltredPaginatedQuery,
    ApiResponse<GetReportFiltredPaginatedQuery.Result>>
{
    private readonly IMapper _mapper;
    private readonly IReportPostQueryRepository _postReportQueryRepository;
    private readonly IReportUserQueryRepository _userReportQueryRepository;

    public GetReportFiltredPaginatedQueryHandler(IReportPostQueryRepository postReportQueryRepository,
        IReportUserQueryRepository userReportQueryRepository, IMapper mapper)
    {
        _postReportQueryRepository = postReportQueryRepository;
        _userReportQueryRepository = userReportQueryRepository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<GetReportFiltredPaginatedQuery.Result>> Handle(
        GetReportFiltredPaginatedQuery request, CancellationToken cancellationToken)
    {
        if (!request.SubjectId.HasValue) throw new EntityNotFoundException("Reports");

        var reports = await _postReportQueryRepository.GetReportPostsFiltredPaginated(request.PageSize,
            request.Page, request.AuthorId, request.SubjectId, request.Status, cancellationToken);

        if (reports.Any())
            return ApiResponse<GetReportFiltredPaginatedQuery.Result>.Success(
                new GetReportFiltredPaginatedQuery.Result(
                    reports.Select(x => new GetReportFiltredPaginatedQuery.Report(
                        x.Id,
                        x.AuthorId,
                        x.ReportedPostId,
                        x.Message,
                        x.CreatedAt
                    )).ToList()
                ));

        var userReports = await _userReportQueryRepository.GetReportUserFiltredPaginated(request.PageSize,
            request.Page, request.AuthorId, request.SubjectId, request.Status, cancellationToken);
        return ApiResponse<GetReportFiltredPaginatedQuery.Result>.Success(new GetReportFiltredPaginatedQuery.Result(
            userReports.Select(x => new GetReportFiltredPaginatedQuery.Report(
                x.Id,
                x.AuthorId,
                x.ReportedUserId,
                x.Message,
                x.CreatedAt
            )).ToList()
        ));
    }
}