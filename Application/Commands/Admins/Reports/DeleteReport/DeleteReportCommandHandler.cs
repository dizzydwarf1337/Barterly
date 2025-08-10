using Application.Core.ApiResponse;
using Domain.Interfaces.Commands.Post;
using Domain.Interfaces.Commands.User;
using MediatR;

namespace Application.Commands.Admins.Reports.DeleteReport;

public class DeleteReportCommandHandler : IRequestHandler<DeleteReportCommand, ApiResponse<Unit>>
{
    private readonly IReportPostCommandRepository _reportPostCommandRepository;
    private readonly IReportUserCommandRepository _reportUserCommandRepository;

    public DeleteReportCommandHandler(IReportPostCommandRepository reportPostCommandRepository,
        IReportUserCommandRepository reportUserCommandRepository)
    {
        _reportPostCommandRepository = reportPostCommandRepository;
        _reportUserCommandRepository = reportUserCommandRepository;
    }

    public async Task<ApiResponse<Unit>> Handle(DeleteReportCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _reportPostCommandRepository.DeleteReportPostAsync(request.ReportId, cancellationToken);
        }
        catch (Exception)
        {
            await _reportUserCommandRepository.DeleteReport(request.ReportId, cancellationToken);
        }

        return ApiResponse<Unit>.Success(Unit.Value);
    }
}