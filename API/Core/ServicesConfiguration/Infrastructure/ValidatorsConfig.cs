using Application.Core.Validators.Implementation;
using Application.Core.Validators.Interfaces;

namespace API.Core.ServicesConfiguration.Infrastructure
{
    public static class ValidatorsConfig
    {
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IAuthChecker, AuthChecker>();
            services.AddScoped<IAccountOwnershipValidator, AccountOwnershipValidator>();
            services.AddScoped<IOpinionOwnershipValidator,OpinionOwnershipValidator>();
            services.AddScoped<IPostOwnershipValidator, PostOwnershipValidator>();
            services.AddScoped<IMessageProfanityValidator, MessageProfanityValidator>();
            return services;
        }
    }
}
