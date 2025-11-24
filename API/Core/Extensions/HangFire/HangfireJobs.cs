using Application.Core.HangfireJobs.PromotionCancelling;
using Application.Core.HangfireJobs.UserActivitySummary;
using Hangfire;

namespace API.Core.Extensions.HangFire;

public static class HangfireJobsRegistrar
{
    public static void Register()
    {
        RecurringJob.AddOrUpdate<IPromotionCancellingJob>(
            "Promotion-cancelling",
            job => job.Execute(),
            Cron.Daily
        );
        RecurringJob.AddOrUpdate<IUserActivitySummaryJob>(
            "Update-user-activity",
            job => job.Execute(),
            Cron.Daily
            );
    }
}