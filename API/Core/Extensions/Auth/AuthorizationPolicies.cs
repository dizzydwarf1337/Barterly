namespace API.Core.Extensions.Auth;

public static class AuthorizationPolicies
{
    public static IServiceCollection AddAppAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(opt =>
        {
            opt.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
            opt.AddPolicy("User", policy => policy.RequireRole("User"));
            opt.AddPolicy("Moderator", policy => policy.RequireRole("Moderator"));
            opt.AddPolicy("All", policy => policy.RequireRole("Admin", "User", "Moderator"));
        });

        return services;
    }
}