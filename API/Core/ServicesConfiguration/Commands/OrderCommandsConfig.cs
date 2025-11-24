using Domain.Interfaces.Commands.Orders;
using Persistence.Repositories.Commands.Orders;

namespace API.Core.ServicesConfiguration.Commands;

public static class OrderCommandsConfig
{

    public static IServiceCollection AddOrderCommands(this IServiceCollection services)
    {
        services.AddScoped<IOrderCommandRepository, OrderCommandRepository>();
        return services;
    }
}