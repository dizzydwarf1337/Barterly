using API.Core.ApiResponse;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : Controller
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();


        protected IActionResult HandleResponse<T>(ApiResponse<T> result)
        {
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return NotFound(result);
            
        }
    }
}
