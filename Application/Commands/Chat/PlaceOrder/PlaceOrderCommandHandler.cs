using Domain.Entities.Orders;
using Domain.Entities.Orders.Types;
using Domain.Interfaces.Commands.Orders;
using Domain.Interfaces.Queries.Orders;
using Domain.Interfaces.Queries.Post;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.Chat.PlaceOrder;

public class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand, bool>
{
    private readonly IOrderCommandRepository _orderCommandRepository;
    private readonly IOrderQueryRepository _orderQueryRepository;
    private readonly IPostQueryRepository _postQueryRepository;

    public PlaceOrderCommandHandler(IOrderCommandRepository orderCommandRepository,
        IOrderQueryRepository orderQueryRepository,
        IPostQueryRepository postQueryRepository)
    {
        _orderCommandRepository = orderCommandRepository;
        _orderQueryRepository = orderQueryRepository;
        _postQueryRepository = postQueryRepository;
    }
    public async Task<bool> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
    {
        var existingOrder = await _orderQueryRepository.GetOrders().FirstOrDefaultAsync(x =>
            x.SellerId == request.SellerId && x.CustomerId == request.CustomerId && x.PostId == request.PostId &&
            x.Status != OrderStatus.Canceled && x.Status != OrderStatus.Delivered, cancellationToken);
        if (existingOrder != null) return false;
         
        var existingPost = await _postQueryRepository.GetAllPosts().Where(x=> x.Id == request.PostId && x.OwnerId == request.SellerId).FirstOrDefaultAsync(cancellationToken);
        if (existingPost == null) return false;
        
        var order = new Order
        {
            CustomerId = request.CustomerId,
            SellerId = request.SellerId,
            PostId = request.PostId,
            Price = request.Price
        };
        await _orderCommandRepository.CreateOrder(order, cancellationToken);
        return true;
    }
}