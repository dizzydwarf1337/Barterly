namespace API.Core.Extensions.Cors;

public static class CorsPolicies
{
    public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
    {
        services.AddCors(opt =>
        {
            opt.AddPolicy("CorsPolicy",
                policy => { 
                    policy
                    .WithOrigins("http://localhost:3000")
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .AllowAnyMethod();
                });
        });

        return services;
    }
}