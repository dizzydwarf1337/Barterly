using Domain.Interfaces.Queries.General;
using Domain.Interfaces.Queries.Post;
using Domain.Interfaces.Queries.User;
using Persistence.Repositories.Queries.General;
using Persistence.Repositories.Queries.Post;
using Persistence.Repositories.Queries.Users;

namespace API.Core.ServicesConfiguration.Queries
{
    public  static class GeneralQueriesConfig
    {
        public static IServiceCollection AddGeneralQueries(this IServiceCollection services)
        {
            //General queries
            services.AddScoped<ILogQueryRepository, LogQueryRepository>();
            services.AddScoped<ICategoryQueryRepository, CategoryQueryRepository>();
            services.AddScoped<IGlobalNotificationQueryRepository, GlobalNotificationQueryRepository>();
            services.AddScoped<INotificationQueryRepository, NotificationQueryRepository>();

            return services;
        }
    }
}
