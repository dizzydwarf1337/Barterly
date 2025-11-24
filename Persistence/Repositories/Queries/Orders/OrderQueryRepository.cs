using Domain.Entities.Orders;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Queries.Orders;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories.Queries.Orders;

public class OrderQueryRepository : BaseQueryRepository<BarterlyDbContext>, IOrderQueryRepository
{
    public OrderQueryRepository(BarterlyDbContext context) : base(context) { }
    
    public IQueryable<Order> GetOrders()
    {
        return _context.Orders.AsQueryable();
    }

    public async Task<Order> GetOrder(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Orders.FirstOrDefaultAsync(x=>x.Id == id, cancellationToken)
            ?? throw new EntityNotFoundException($"Order with id {id} not found");
    }
}