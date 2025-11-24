using Domain.Entities.Orders;

namespace Domain.Interfaces.Commands.Orders;

public interface IOrderCommandRepository
{
    Task<Order> CreateOrder(Order order, CancellationToken cancellationToken);
    Task<Order> UpdateOrder(Order order, CancellationToken cancellationToken);
    Task DeleteOrder(Order order, CancellationToken cancellationToken);
}