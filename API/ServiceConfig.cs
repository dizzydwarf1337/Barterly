using Application.Events.Users;
using Application.Services;

namespace API
{
    public class ServiceConfig
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<AuthService>();
            services.AddScoped<LogService>();
        }
    }
}
