using Application.Core.MediatR.Requests;
using MediatR;

namespace Application.Commands.Users.Orders.MarkAsShipped;

public class MarkAsShippedCommand : UserRequest<Unit>
{
    public Guid OrderId { get; set; }
}