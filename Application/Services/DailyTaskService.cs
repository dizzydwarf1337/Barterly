using Application.Interfaces;
using Domain.Interfaces.Queries.User;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class DailyTaskService : BackgroundService
{
    private readonly ILogger<DailyTaskService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public DailyTaskService(ILogger<DailyTaskService> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await RunTask();

        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.Now;
            var nextRun = DateTime.Today.AddDays(1);
            var delay = nextRun - now;

            _logger.LogInformation($"Waiting {delay} until next run.");

            if (delay.TotalMilliseconds > 0)
                await Task.Delay(delay, stoppingToken);

            if (stoppingToken.IsCancellationRequested)
                break;

            await RunTask();
        }
    }

    private async Task RunTask()
    {
        using var scope = _serviceProvider.CreateScope();
        var userQueryRepository = scope.ServiceProvider.GetRequiredService<IUserQueryRepository>();
        var userActivityService = scope.ServiceProvider.GetRequiredService<IUserActivityService>();

        var usersGuids = await userQueryRepository.GetUsersIdsAsync(DateTime.Now.AddMonths(-1));
        foreach (var userGuid in usersGuids)
        {
            _logger.LogInformation($"Summarizing User: {userGuid}");
            await userActivityService.SummarizeUserActivity(userGuid);
        }
        _logger.LogInformation($"Task running at {DateTime.Now}");
    }
}
