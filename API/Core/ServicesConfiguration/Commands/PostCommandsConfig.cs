using Domain.Interfaces.Commands.Post;
using Persistence.Repositories.Commands.Post;
using Persistence.Repositories.Commands.Posts;

namespace API.Core.ServicesConfiguration.Commands;

public static class PostCommandsConfig
{
    public static IServiceCollection AddPostCommands(this IServiceCollection services)
    {
        services.AddScoped<IPostOpinionCommandRepository, PostOpinionCommandRepository>();
        services.AddScoped<IReportPostCommandRepository, ReportPostCommandRepository>();
        services.AddScoped<IPostCommandRepository, PostCommandRepository>();
        services.AddScoped<IPostImageCommandRepository, PostImageCommandRepository>();
        services.AddScoped<IPromotionCommandRepository, PromotionCommandRepository>();
        services.AddScoped<IPostSettingsCommandRepository, PostSettingsCommandRepository>();
        return services;
    }
}