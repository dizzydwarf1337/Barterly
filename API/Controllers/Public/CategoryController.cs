using Application.Queries.Public.Categories.GetAllCategories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Public;

[Route("public/categories")]
[AllowAnonymous]
public class CategoryController : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        return HandleResponse(await Mediator.Send(new GetAllCategoriesQuery()));
    }
}