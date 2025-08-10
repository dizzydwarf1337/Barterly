using Application.Core.ApiResponse;
using Application.Events.Reports.ReportReviewedEvent;
using Domain.Entities.Common;
using Domain.Interfaces.Commands.Post;
using Domain.Interfaces.Commands.User;
using MediatR;

namespace Application.Commands.Moderators.Reports.ReviewReport;

public class ReivewReportCommandHandler : IRequestHandler<ReviewReportCommand, ApiResponse<Report>>
{
    private readonly IMediator _mediator;
    private readonly IReportPostCommandRepository _postReportCommandRepotory;
    private readonly IReportUserCommandRepository _userReportCommandRepository;

    public ReivewReportCommandHandler(IReportUserCommandRepository reportUserCommandRepository,
        IReportPostCommandRepository reportPostCommandRepository, IMediator mediator)
    {
        _userReportCommandRepository = reportUserCommandRepository;
        _postReportCommandRepotory = reportPostCommandRepository;
        _mediator = mediator;
    }

    public async Task<ApiResponse<Report>> Handle(ReviewReportCommand request, CancellationToken cancellationToken)
    {
        Report report;
        try
        {
            report = await _postReportCommandRepotory.ReviewReport(request.ReportId, request.ReportStatus,
                request.AuthorizeData.UserId, cancellationToken);
        }
        catch (Exception)
        {
            report = await _userReportCommandRepository.ReviewReport(request.ReportId, request.ReportStatus,
                request.AuthorizeData.UserId, cancellationToken);
        }

        await _mediator.Publish(new ReportReviewedEvent { Report = report });

        return ApiResponse<Report>.Success(report);
    }
}