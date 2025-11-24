using Domain.Entities.Orders;
using Domain.Interfaces.Commands.Orders;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories.Commands.Orders;

public class OrderCommandRepository : BaseCommandRepository<BarterlyDbContext>,  IOrderCommandRepository
{
    public OrderCommandRepository(BarterlyDbContext context) : base(context) { }
    
    public async Task<Order> CreateOrder(Order order, CancellationToken cancellationToken)
    {
        await _context.Orders.AddAsync(order, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return order;
    }

    public async Task<Order> UpdateOrder(Order order, CancellationToken cancellationToken)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync(cancellationToken);
        return order;
    }

    public async Task DeleteOrder(Order order, CancellationToken cancellationToken)
    {
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync(cancellationToken);
    }
}