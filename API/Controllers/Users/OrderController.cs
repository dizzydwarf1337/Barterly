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
}