using Application.Core.ApiResponse;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api")]
[ApiController]
public class BaseController : Controller
{
    private IMediator _mediator;
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();


    protected IActionResult HandleResponse<T>(ApiResponse<T> result)
    {
        return StatusCode(result.StatusCode, result);
    }
}