using MediatR;

namespace Application.Commands.Chat.PlaceOrder;

public class PlaceOrderCommand : IRequest<bool>
{
    public Guid CustomerId { get; set; }
    public Guid SellerId { get; set; }
    public Guid PostId { get; set; }
    
    public decimal Price { get; set; }
}