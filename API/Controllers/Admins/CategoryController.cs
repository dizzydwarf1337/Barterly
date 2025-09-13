using Application.Commands.Admins.Categories.AddCategory;
using Application.Commands.Admins.Categories.DeleteCategory;
using Application.Commands.Admins.Categories.EditCategory;
using Application.Queries.Admins.Categories.GetAllCategories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("admin/categories")]
[Authorize(Policy = "Admin")]
public class AdminCategoryController : BaseController
{
    [HttpPost]
    public async Task<IActionResult> GetAllCategories(GetAllCategoriesQuery request)
    {
        return HandleResponse(await Mediator.Send(request));
    }

    [HttpPost]
    [Route("create-category")]
    public async Task<IActionResult> AddCategory(AddCategoryCommand command)
    {
        return HandleResponse(await Mediator.Send(command));
    }

    [HttpDelete]
    [Route("delete/{id:guid}")]
    public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
    {
        return HandleResponse(await Mediator.Send(new DeleteCategoryCommand { CategoryId = id }));
    }

    [HttpPut]
    [Route("update-category")]
    public async Task<IActionResult> EditCategory(EditCategoryCommand command)
    {
        return HandleResponse(await Mediator.Send(command));
    }
}