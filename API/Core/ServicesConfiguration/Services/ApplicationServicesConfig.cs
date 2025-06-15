using Application.Interfaces;
using Application.Services;

namespace API.Core.ServicesConfiguration.Services
{
    public static class ApplicationServicesConfig
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services) 
        {


            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IMailService, GmailService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ILogService, LogService>();
            services.AddScoped<IRecommendationService, RecommendationService>();
            services.AddScoped<IUserActivityService, UserActivityService>();
            return services;
        }
    }
}
