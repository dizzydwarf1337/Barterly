using Domain.Interfaces.Commands.Post;
using Domain.Interfaces.Commands.User;
using Persistence.Repositories.Commands.Post;
using Persistence.Repositories.Commands.Posts;
using Persistence.Repositories.Commands.Users;

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
        services.AddScoped<IUserFavPostCommandRepository, UserFavPostCommandRepository>();
        return services;
    }
}