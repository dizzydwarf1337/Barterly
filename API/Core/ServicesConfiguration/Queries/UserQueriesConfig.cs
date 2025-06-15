using Domain.Interfaces.Queries.Post;
using Domain.Interfaces.Queries.User;
using Persistence.Repositories.Queries.Posts;
using Persistence.Repositories.Queries.Users;

namespace API.Core.ServicesConfiguration.Queries
{
    public static class UserQueriesConfig
    {
        public static IServiceCollection AddUserQueries(this IServiceCollection services)
        {

            //Users query
            services.AddScoped<IUserActivityQueryRepository, UserActivityQueryRepository>();
            services.AddScoped<IUserOpinionQueryRepository, UserOpinionQueryRepository>();
            services.AddScoped<IUserSettingQueryRepository, UserSettingQueryRepository>();
            services.AddScoped<IReportUserQueryRepository, ReportUserQueryRepository>();
            services.AddScoped<IVisitedPostQueryRepository, VisitedPostQueryRepository>();
            services.AddScoped<IUserQueryRepository, UserQueryRepository>();
            return services;
        }
    }
}
