using API.Core.ApiResponse;
using Application.DTOs.Reports;
using AutoMapper;
using Domain.Entities.Common;
using Domain.Interfaces.Commands.User;
using Domain.Interfaces.Queries.Post;
using Domain.Interfaces.Queries.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Reports.Queries.GetReportsFiltredPaginated
{
    public class GetReportFiltredPaginatedQueryHandler : IRequestHandler<GetReportFiltredPaginatedQuery, ApiResponse<ICollection<ReportDto>>>
    {
        private readonly IReportPostQueryRepository _postReportQueryRepository;
        private readonly IReportUserQueryRepository _userReportQueryRepository;
        private readonly IMapper _mapper;

        public GetReportFiltredPaginatedQueryHandler(IReportPostQueryRepository postReportQueryRepository, IReportUserQueryRepository userReportQueryRepository, IMapper mapper)
        {
            _postReportQueryRepository = postReportQueryRepository;
            _userReportQueryRepository = userReportQueryRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<ICollection<ReportDto>>> Handle(GetReportFiltredPaginatedQuery request, CancellationToken cancellationToken)
        {
            List<Report> reports = new List<Report>();
            reports.AddRange(await _postReportQueryRepository.GetReportPostsFiltredPaginated(request.Page, (int)request.PageSize/2, request.AuthorId, request.SubjectId, request.Status));
            reports.AddRange(await _userReportQueryRepository.GetReportUserFiltredPaginated(request.Page,(int)request.PageSize/2,request.AuthorId,request.SubjectId, request.Status));
            return ApiResponse<ICollection<ReportDto>>.Success(_mapper.Map<List<ReportDto>>(reports)); 
        }
    }
}
