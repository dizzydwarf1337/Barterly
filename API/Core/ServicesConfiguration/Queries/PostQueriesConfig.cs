using Domain.Interfaces.Queries.Post;
using Persistence.Repositories.Queries.Post;
using Persistence.Repositories.Queries.Posts;

namespace API.Core.ServicesConfiguration.Queries
{
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

            return services;
        }
    }
}
