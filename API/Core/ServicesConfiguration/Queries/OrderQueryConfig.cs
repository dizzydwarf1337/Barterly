using Domain.Interfaces.Queries.Orders;
using Persistence.Repositories.Queries.Orders;

namespace API.Core.ServicesConfiguration.Queries;

public static class OrderQueryConfig
{
    public static IServiceCollection AddOrderQueries(this IServiceCollection services)
    {
        services.AddScoped<IOrderQueryRepository, OrderQueryRepository>();
        return services;
    }
}