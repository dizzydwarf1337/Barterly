using Domain.Enums.Posts;
using Domain.Interfaces.Commands.Post;
using Domain.Interfaces.Queries.Post;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Core.HangfireJobs.PromotionCancelling;

public class PromotionCancellingJob : IPromotionCancellingJob
{
    private readonly IServiceScopeFactory _scopeFactory;

    public PromotionCancellingJob(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task Execute()
    {
        using var scope = _scopeFactory.CreateScope();
        var promotionQueryRepository = scope.ServiceProvider.GetRequiredService<IPromotionQueryRepository>();
        var promotions = await promotionQueryRepository.GetPromotions()
            .Where(x => x.EndDate < DateTime.UtcNow)
            .ToListAsync();

        foreach (var promotion in promotions)
        {
            using var innerScope = _scopeFactory.CreateScope();
            var promotionCommandRepository = innerScope.ServiceProvider.GetRequiredService<IPromotionCommandRepository>();

            promotion.StartDate = DateTime.UtcNow;
            promotion.EndDate = DateTime.MaxValue;
            promotion.Type = PostPromotionType.None;

            await promotionCommandRepository.UpdatePromotionAsync(promotion, CancellationToken.None);
        }
    }
}