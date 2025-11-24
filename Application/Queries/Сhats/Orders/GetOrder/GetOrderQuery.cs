using Domain.Entities.Orders;
using MediatR;

namespace Application.Queries.Сhats.Orders.GetOrder;

public class GetOrderQuery : IRequest<Order?>
{
    public Guid BuyerId { get; set; }
    public Guid SellerId { get; set; }
    public Guid PostId { get; set; }
}