using API.Core.ApiResponse;
using Application.DTOs.Reports;
using Domain.Enums.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Reports.Queries.GetReportsFiltredPaginated
{
    public class GetReportFiltredPaginatedQuery : IRequest<ApiResponse<ICollection<ReportDto>>> 
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public Guid? AuthorId { get; set; }
        public Guid? SubjectId { get; set; }
        public ReportStatusType? Status { get; set; }
    }
}
