using Application.Commands.Admins.Categories.AddCategory;
using Application.Commands.Admins.Categories.DeleteCategory;
using Application.Commands.Admins.Categories.EditCategory;
using Application.Queries.Moderators.Categories.GetAllCategories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("moderator/categories")]
[Authorize(Policy = "Moderator")]
public class ModeratorCategoryController : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        return HandleResponse(await Mediator.Send(new GetAllCategoriesQuery()));
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