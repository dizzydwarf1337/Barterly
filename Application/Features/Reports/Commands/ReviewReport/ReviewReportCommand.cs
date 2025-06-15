using API.Core.ApiResponse;
using Application.DTOs.Reports;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Reports.Commands.ReviewReport
{
    public class ReviewReportCommand : IRequest<ApiResponse<ReportDto>>
    {
        public required ReviewReportDto ReviewDto { get; set; }
        public required string AuthoritiesId { get; set; } 
    }
}
