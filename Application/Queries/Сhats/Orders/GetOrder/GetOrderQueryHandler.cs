using Domain.Entities.Orders;
using Domain.Entities.Orders.Types;
using Domain.Interfaces.Queries.Orders;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Сhats.Orders.GetOrder;

public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, Order?>
{
    private readonly IOrderQueryRepository _orderQueryRepository;
    
    public GetOrderQueryHandler(IOrderQueryRepository orderQueryRepository)
        => _orderQueryRepository = orderQueryRepository;
    
    public async Task<Order?> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        return await _orderQueryRepository.GetOrders().Where(x =>
            x.SellerId == request.SellerId && x.CustomerId == request.BuyerId && x.PostId == request.PostId && x.Status != OrderStatus.Canceled && x.Status != OrderStatus.Shipped)
            .FirstOrDefaultAsync(cancellationToken);
    }
}