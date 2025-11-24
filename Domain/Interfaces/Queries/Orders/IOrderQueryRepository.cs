using Domain.Entities.Orders;

namespace Domain.Interfaces.Queries.Orders;

public interface IOrderQueryRepository
{
    IQueryable<Order> GetOrders();
    Task<Order> GetOrder(Guid id,  CancellationToken cancellationToken);
}