using Application.Core.ApiResponse;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Commands.Post;
using Domain.Interfaces.Commands.User;
using MediatR;

namespace Application.Commands.Moderators.Reports.DeleteReport;

public class DeleteReportCommandHandler : IRequestHandler<DeleteReportCommand, ApiResponse<Unit>>
{
    private readonly IReportPostCommandRepository _reportpostCommandRepository;
    private readonly IReportUserCommandRepository _reportUserCommandRepository;

    public DeleteReportCommandHandler(IReportUserCommandRepository reportUserCommandRepository,
        IReportPostCommandRepository reportpostCOmmandRepository)
    {
        _reportUserCommandRepository = reportUserCommandRepository;
        _reportpostCommandRepository = reportpostCOmmandRepository;
    }

    public async Task<ApiResponse<Unit>> Handle(DeleteReportCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _reportpostCommandRepository.DeleteReportPostAsync(request.ReportId, cancellationToken);
        }
        catch (EntityNotFoundException)
        {
            await _reportUserCommandRepository.DeleteReport(request.ReportId, cancellationToken);
        }

        return ApiResponse<Unit>.Success(Unit.Value);
    }
}