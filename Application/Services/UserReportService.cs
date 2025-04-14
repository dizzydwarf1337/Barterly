using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Commands.User;
using Domain.Interfaces.Queries.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserReportService : IReportService
    {
        private readonly IMapper _mapper;
        private readonly IReportUserCommandRepository _reportUserCommandRepository;
        private readonly IReportUserQueryRepository _reportUserQueryRepository;

        public UserReportService(IReportUserCommandRepository reportUserCommandRepository, IReportUserQueryRepository reportUserQueryRepository, IMapper mapper)
        {
            _reportUserCommandRepository = reportUserCommandRepository;
            _reportUserQueryRepository = reportUserQueryRepository;
            _mapper = mapper;
        }

        public async Task ChangeReportStatus(Guid reportId, ReportStatusType reportStatusType)
        {
            await _reportUserCommandRepository.ChangeReportStatus(reportId, reportStatusType);
        }

        public async Task DeleteReport(Guid reportId)
        {
            await _reportUserCommandRepository.DeleteReport(reportId);
        }

        public async Task<ICollection<ReportDto>> GetAllReports()
        {
            var reportDtos = _mapper.Map<ICollection<ReportDto>>(await _reportUserQueryRepository.GetAllUsersReportsAsync());
            return reportDtos;
        }

        public async Task<ICollection<ReportDto>> GetReportsByAuthorId(Guid authorId)
        {
            var reportsDtos = _mapper.Map<ICollection<ReportDto>>(await _reportUserQueryRepository.GetReportUsersByAuthorIdAsync(authorId));
            return reportsDtos;
        }

        public  async Task<ICollection<ReportDto>> GetReportsByRepotedSubjectId(Guid repotedSubjectId)
        {
            var reportsDtos = _mapper.Map<ICollection<ReportDto>>(await _reportUserQueryRepository.GetReportsUserByUserIdAsync(repotedSubjectId));
            return reportsDtos;
        }

        public async Task SendReport(ReportDto reportDto)
        {
            var report = _mapper.Map<ReportUser>(reportDto);
            await _reportUserCommandRepository.CreateReport(report);
        }
    }
}
