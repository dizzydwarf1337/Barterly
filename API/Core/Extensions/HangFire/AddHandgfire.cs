using Application.Core.HangfireJobs.PromotionCancelling;
using Application.Core.HangfireJobs.UserActivitySummary;
using Hangfire;

namespace API.Core.Extensions.HangFire;

public static class AddHangfire
{
    public static IServiceCollection AddHangFireConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfire(config =>
        {
            config.UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection"));
        });
        services.AddHangfireServer();

        services.AddScoped<IPromotionCancellingJob,  PromotionCancellingJob>();
        services.AddScoped<IUserActivitySummaryJob, UserActivitySummaryJob>();
        return services;
    }
}