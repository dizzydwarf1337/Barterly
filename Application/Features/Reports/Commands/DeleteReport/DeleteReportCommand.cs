using API.Core.ApiResponse;
using Application.Interfaces.CommandInterfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Reports.Commands.DeleteReport
{
    public class DeleteReportCommand : IRequest<ApiResponse<Unit>>
    {
        public required string ReportId { get; set; }
    }
}
