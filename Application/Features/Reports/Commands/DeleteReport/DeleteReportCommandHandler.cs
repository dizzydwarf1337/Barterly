using API.Core.ApiResponse;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Commands.Post;
using Domain.Interfaces.Commands.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Reports.Commands.DeleteReport
{
    public class DeleteReportCommandHandler : IRequestHandler<DeleteReportCommand, ApiResponse<Unit>>
    {
        private readonly IReportUserCommandRepository _reportUserCommandRepository;
        private readonly IReportPostCommandRepository _reportpostCommandRepository;

        public DeleteReportCommandHandler(IReportUserCommandRepository reportUserCommandRepository, IReportPostCommandRepository reportpostCOmmandRepository)
        {
            _reportUserCommandRepository = reportUserCommandRepository;
            _reportpostCommandRepository = reportpostCOmmandRepository;
        }

        public async Task<ApiResponse<Unit>> Handle(DeleteReportCommand request, CancellationToken cancellationToken)
        {
            var reportId = Guid.Parse(request.ReportId);
            try
            {
                await _reportpostCommandRepository.DeleteReportPostAsync(reportId);
            }
            catch (EntityNotFoundException)
            {
                await _reportUserCommandRepository.DeleteReport(reportId);
            }
            return ApiResponse<Unit>.Success(Unit.Value);
        }
    }
}
