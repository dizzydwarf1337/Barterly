using Application.Commands.Users.Orders.MarkAsDelivered;
using Application.Commands.Users.Orders.MarkAsShipped;
using Application.Queries.Users.Orders.GetMyOrders;
using Application.Queries.Users.Orders.GetMyPlacedOrders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Users;

[Route("users/order")]
[Authorize(Policy = "User")]
public class OrderController : BaseController
{
    [HttpGet]
    [Route("my")]
    public async Task<IActionResult> GetMyOrders()
        => HandleResponse(await Mediator.Send(new GetMyOrdersQuery()));

    [HttpGet]
    [Route("my-placed")]
    public async Task<IActionResult> GetMyPlacedOrders()
        => HandleResponse(await Mediator.Send(new GetMyPlacedOrdersQuery()));

    [HttpPut]
    [Route("deliver/{orderId:guid}")]
    public async Task<IActionResult> MarkAsDelivered([FromRoute] Guid orderId)
    {
        var command = new MarkAsDeliveredCommand
        {
            OrderId = orderId
        };
        
        return HandleResponse(await Mediator.Send(command));
    }

    [HttpPut]
    [Route("ship/{orderId:guid}")]
    public async Task<IActionResult> MarkAsShipped([FromRoute] Guid orderId)
    {
        var command = new MarkAsShippedCommand
        {
            OrderId = orderId
        };
        
        return HandleResponse(await Mediator.Send(command));
    }
}