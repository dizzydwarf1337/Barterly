using Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Persistence.Database;

namespace API.Core.Extensions.Identity;

public static class IdentityServices
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {
        services.AddIdentity<User, IdentityRole<Guid>>(options => { options.SignIn.RequireConfirmedAccount = false; })
            .AddEntityFrameworkStores<BarterlyDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }
}