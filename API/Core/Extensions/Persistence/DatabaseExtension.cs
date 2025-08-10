using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace API.Core.Extensions.Persistence;

public static class DatabaseExtension
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<BarterlyDbContext>(opt =>
            opt.UseSqlServer(config.GetConnectionString("DefaultConnection")));

        return services;
    }
}