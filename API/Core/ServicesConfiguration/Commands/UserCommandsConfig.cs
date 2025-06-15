using Domain.Interfaces.Commands.Post;
using Domain.Interfaces.Commands.User;
using Persistence.Repositories.Commands.Post;
using Persistence.Repositories.Commands.Users;

namespace API.Core.ServicesConfiguration.Commands
{
    public static class UserCommandsConfig
    {
        public static IServiceCollection AddUserCommands(this IServiceCollection services) 
        {
            //User commands
            services.AddScoped<IUserActivityCommandRepository, UserActivityCommandRepository>();
            services.AddScoped<IUserOpinionCommandRepository, UserOpinionCommandRepository>();
            services.AddScoped<IReportUserCommandRepository, ReportUserCommandRepository>();
            services.AddScoped<IVisitedPostCommandRepository, VisitedPostCommandRepository>();
            services.AddScoped<IUserSettingCommandRepository, UserSettingCommandRepository>();
            services.AddScoped<IUserCommandRepository, UserCommandRepository>();
            return services; 

        }
    }
}
