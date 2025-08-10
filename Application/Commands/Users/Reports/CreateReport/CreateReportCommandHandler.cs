using Application.Core.ApiResponse;
using Application.Core.Factories.Interfaces;
using MediatR;

namespace Application.Commands.Users.Reports.CreateReport;

public class CreateReportCommandHandler : IRequestHandler<CreateReportCommand, ApiResponse<Unit>>
{
    private readonly IReportFactory _reportFactory;

    public CreateReportCommandHandler(IReportFactory reportFactory)
    {
        _reportFactory = reportFactory;
    }

    public async Task<ApiResponse<Unit>> Handle(CreateReportCommand request, CancellationToken cancellationToken)
    {
        await _reportFactory.CreateReportAsync(request.OwnerId, request.SubjectId, request.Message,
            request.ReportType, cancellationToken);

        return ApiResponse<Unit>.Success(Unit.Value, 201);
    }
}