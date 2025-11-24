namespace Application.Core.HangfireJobs.UserActivitySummary;

public interface IUserActivitySummaryJob
{
    Task Execute();
}