using API.Core.ApiResponse;
using Application.DTOs.Reports;
using Application.Features.Reports.Commands.ReviewReport;
using Application.Features.Reports.Events.ReportReviewdEvent;
using AutoMapper;
using Domain.Entities.Common;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Commands.Post;
using Domain.Interfaces.Commands.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Reports.Commands.UpdateReport
{
    public class ReivewReportCommandHandler : IRequestHandler<ReviewReportCommand, ApiResponse<ReportDto>>
    {
        private readonly IReportUserCommandRepository _reportUserCommandRepository;
        private readonly IReportPostCommandRepository _reportPostCommandRepository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        public ReivewReportCommandHandler(IReportUserCommandRepository reportUserCommandRepository, IReportPostCommandRepository reportPostCommandRepository, IMapper mapper, IMediator mediator)
        {
            _reportUserCommandRepository = reportUserCommandRepository;
            _reportPostCommandRepository = reportPostCommandRepository;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<ApiResponse<ReportDto>> Handle(ReviewReportCommand request, CancellationToken cancellationToken)
        {
            var reportId = Guid.Parse(request.ReviewDto.ReportId);
            Report report;
            try
            {
                report = await _reportPostCommandRepository.ReviewReport(reportId, request.ReviewDto.ReportStatus, request.AuthoritiesId);
            }
            catch (EntityNotFoundException)
            {
                report = await _reportUserCommandRepository.ReviewReport(reportId, request.ReviewDto.ReportStatus, request.AuthoritiesId);
            }
            await _mediator.Publish(new ReportReviewedEvent { Report = report });
            return ApiResponse<ReportDto>.Success(_mapper.Map<ReportDto>(report));
        }
    }
}
