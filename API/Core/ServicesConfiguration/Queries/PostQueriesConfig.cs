using Domain.Interfaces.Queries.Post;
using Domain.Interfaces.Queries.User;
using Persistence.Repositories.Queries.Post;
using Persistence.Repositories.Queries.Posts;
using Persistence.Repositories.Queries.Users;

namespace API.Core.ServicesConfiguration.Queries;

public static class PostQueriesConfig
{
    public static IServiceCollection AddPostQueries(this IServiceCollection services)
    {
        //Posts query
        services.AddScoped<IPostOpinionQueryRepository, PostOpinionQueryRepository>();
        services.AddScoped<IReportPostQueryRepository, ReportPostQueryRepository>();
        services.AddScoped<IPostQueryRepository, PostQueryRepository>();
        services.AddScoped<IPostImageQueryRepository, PostImageQueryRepository>();
        services.AddScoped<IPromotionQueryRepository, PromotionQueryRepository>();
        services.AddScoped<IPostSettingsQueryRepository, PostSettingsQueryRepository>();
        services.AddScoped<IUserFavPostQueryRepository, UserFavPostQueryRepository>();
        return services;
    }
}