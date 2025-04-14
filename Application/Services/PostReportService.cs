using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Commands.Post;
using Domain.Interfaces.Queries.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PostReportService : IReportService
    {
        private readonly IReportPostCommandRepository _reportPostCommandRepository;
        private readonly IReportPostQueryRepository _reportPostQueryRepository;
        private readonly IMapper _mapper;
        private readonly ILogService _logService;

        public PostReportService(IReportPostCommandRepository reportPostCommandRepository, IReportPostQueryRepository reportPostQueryRepository, IMapper mapper, ILogService logService)
        {
            _reportPostCommandRepository = reportPostCommandRepository;
            _reportPostQueryRepository = reportPostQueryRepository;
            _mapper = mapper;
            _logService = logService;
        }

        public async Task ChangeReportStatus(Guid reportId, ReportStatusType reportStatusType)
        {
            await _reportPostCommandRepository.ChangeReportPostStatusAsync(reportId, reportStatusType);
            await _logService.CreateLogAsync($"Report with id {reportId} changed status to {reportStatusType} ", LogType.Information, null, null, null);
        }

        public async Task DeleteReport(Guid reportId)
        {
            await _reportPostCommandRepository.DeleteReportPostAsync(reportId);
            await _logService.CreateLogAsync($"Report with id {reportId} deleted", LogType.Information, null, null, null);
        }

        public async Task<ICollection<ReportDto>> GetAllReports()
        {
            return _mapper.Map<ICollection<ReportDto>>(await _reportPostQueryRepository.GetAllPostReportsAsync());
        }

        public async Task<ICollection<ReportDto>> GetReportsByAuthorId(Guid authorId)
        {
            return _mapper.Map<ICollection<ReportDto>>(await _reportPostQueryRepository.GetReportPostsByAuthorIdAsync(authorId));
        }

        public async Task<ICollection<ReportDto>> GetReportsByRepotedSubjectId(Guid repotedSubjectId)
        {
            return _mapper.Map<ICollection<ReportDto>>(await _reportPostQueryRepository.GetReportsPostByPostIdAsync(repotedSubjectId));
        }


        public async Task SendReport(ReportDto reportDto)
        {
            var report = _mapper.Map<ReportPost>(reportDto);
            await _reportPostCommandRepository.CreateReportPostAsync(report);
            await _logService.CreateLogAsync($"Report created with id {report.Id} ", LogType.Information, null, report.ReportedPostId, report.AuthorId);
        }
    }
}
