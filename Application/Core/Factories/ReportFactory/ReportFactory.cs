using Application.Core.Factories.Interfaces;
using Application.DTOs.Reports;
using AutoMapper;
using Domain.Entities.Common;
using Domain.Entities.Posts;
using Domain.Entities.Users;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Commands.Post;
using Domain.Interfaces.Commands.User;

namespace Application.Core.Factories.ReportFactory
{
    public class ReportFactory : IReportFactory
    {
        private readonly IReportPostCommandRepository _postReportCommandRepository;
        private readonly IReportUserCommandRepository _userReportCommmandRepository;
        private readonly IMapper _mapper;

        public ReportFactory(IReportPostCommandRepository postReportCommandRepository, IReportUserCommandRepository userReportCommmandRepository, IMapper mapper)
        {
            _postReportCommandRepository = postReportCommandRepository;
            _userReportCommmandRepository = userReportCommmandRepository;
            _mapper = mapper;
        }

        public async Task CreateReportAsync(CreateReportDto createReportDto)
        {
            if (createReportDto.ReportType == "Post")
            {
                var report = _mapper.Map<ReportPost>(createReportDto);
                await _postReportCommandRepository.CreateReportPostAsync(report);
            }
            else if (createReportDto.ReportType == "User")
            {
                var report = _mapper.Map<ReportUser>(createReportDto);
                await _userReportCommmandRepository.CreateReport(report);
            }
            else throw new EntityCreatingException("Report","ReportFactory");
        }
    }
}
