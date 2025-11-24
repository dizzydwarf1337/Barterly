using Application.Interfaces;
using Domain.Interfaces.Queries.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Core.HangfireJobs.UserActivitySummary;

public class UserActivitySummaryJob : IUserActivitySummaryJob
{
    private readonly IServiceScopeFactory _scopeFactory;

    public UserActivitySummaryJob(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task Execute()
    {
        using var scope = _scopeFactory.CreateScope();
        var userQueryRepository = scope.ServiceProvider.GetRequiredService<IUserQueryRepository>();
        var users = await userQueryRepository.GetUsers()
            .Where(x => x.EmailConfirmed && !x.Setting!.IsBanned && !x.Setting.IsDeleted)
            .ToListAsync();

        foreach (var user in users)
        {
            using var innerScope = _scopeFactory.CreateScope();
            var activityService = innerScope.ServiceProvider.GetRequiredService<IUserActivityService>();
            await activityService.SummarizeUserActivity(user.Id, CancellationToken.None);
        }
    }
}