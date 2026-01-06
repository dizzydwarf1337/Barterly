using Application.Core.MediatR.Requests;
using MediatR;

namespace Application.Commands.Users.Orders.MarkAsDelivered;

public class MarkAsDeliveredCommand : UserRequest<Unit>
{
    public Guid OrderId { get; set; }
}