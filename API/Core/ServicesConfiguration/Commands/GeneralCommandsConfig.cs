using Domain.Interfaces.Commands.General;
using Domain.Interfaces.Commands.Post;
using Domain.Interfaces.Commands.User;
using Persistence.Repositories.Commands.General;
using Persistence.Repositories.Commands.Post;
using Persistence.Repositories.Commands.Users;
using System.Collections;

namespace API.Core.ServicesConfiguration.Commands
{
    public static class GeneralCommandsConfig 
    {
        public static IServiceCollection AddGeneralCommands(this IServiceCollection services)
        {
            services.AddScoped<ILogCommandRepository, LogCommandRepository>();
            services.AddScoped<ICategoryCommandRepository, CategoryCommandRepository>();
            services.AddScoped<IGlobalNotificationCommandRepository, GlobalNotificationCommandRepository>();
            services.AddScoped<INotificationCommandRepository, NotificationCommandRepository>();
            return services;
        }
    }
}
