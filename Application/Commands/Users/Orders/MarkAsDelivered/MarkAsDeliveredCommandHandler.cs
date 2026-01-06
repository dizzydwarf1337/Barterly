using Application.Core.ApiResponse;
using Domain.Entities.Orders.Types;
using Domain.Interfaces.Commands.Orders;
using Domain.Interfaces.Queries.Orders;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.Users.Orders.MarkAsDelivered;

public class MarkAsDeliveredCommandHandler : IRequestHandler<MarkAsDeliveredCommand, ApiResponse<Unit>>
{
    private readonly IOrderQueryRepository _orderQueryRepository;
    private readonly IOrderCommandRepository _orderCommandRepository;

    public MarkAsDeliveredCommandHandler(IOrderQueryRepository orderQueryRepository,
        IOrderCommandRepository orderCommandRepository)
    {
        _orderQueryRepository = orderQueryRepository;
        _orderCommandRepository = orderCommandRepository;
    }
    
    public async Task<ApiResponse<Unit>> Handle(MarkAsDeliveredCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderQueryRepository.GetOrders()
            .Where(x => x.Id == request.OrderId && x.CustomerId == request.AuthorizeData!.UserId)
            .FirstOrDefaultAsync(cancellationToken);
        
        if(order == null)
            return ApiResponse<Unit>.Failure("Order not found", 404);

        order.Status = OrderStatus.Delivered;

        await _orderCommandRepository.UpdateOrder(order, cancellationToken);
        
        return ApiResponse<Unit>.Success(Unit.Value);
    }
}