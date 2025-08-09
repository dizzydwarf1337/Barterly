namespace Application.Core.Factories.Interfaces;

public interface IReportFactory
{
    Task CreateReportAsync(Guid OwnerId, Guid SubjectId, string Message, string ReportType,
        CancellationToken token);
}