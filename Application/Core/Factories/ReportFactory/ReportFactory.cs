using Application.Core.Factories.Interfaces;
using Domain.Entities.Posts;
using Domain.Entities.Users;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Commands.Post;
using Domain.Interfaces.Commands.User;

namespace Application.Core.Factories.ReportFactory;

public class ReportFactory : IReportFactory
{
    private readonly IReportPostCommandRepository _postReportCommandRepository;
    private readonly IReportUserCommandRepository _userReportCommmandRepository;

    public ReportFactory(IReportPostCommandRepository postReportCommandRepository,
        IReportUserCommandRepository userReportCommmandRepository)
    {
        _postReportCommandRepository = postReportCommandRepository;
        _userReportCommmandRepository = userReportCommmandRepository;
    }

    public async Task CreateReportAsync(Guid OwnerId, Guid SubjectId, string Message, string ReportType,
        CancellationToken token)
    {
        if (ReportType == "Post")
        {
            var report = new ReportPost { Message = Message, AuthorId = OwnerId, ReportedPostId = SubjectId };
            await _postReportCommandRepository.CreateReportPostAsync(report, token);
        }
        else if (ReportType == "User")
        {
            var report = new ReportUser { Message = Message, AuthorId = OwnerId, ReportedUserId = SubjectId };
            await _userReportCommmandRepository.CreateReport(report, token);
        }
        else
        {
            throw new EntityCreatingException("Report", "ReportFactory");
        }
    }
}