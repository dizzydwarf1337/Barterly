using Application.DTOs;
using Application.Features.Category.Commands.AddCategory;
using Application.Features.Category.Commands.DeleteCategory;
using Application.Features.Category.Queries.GetAllCategories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CategoryController : BaseController
    {

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {

            return  HandleResponse(await Mediator.Send(new GetAllCategoriesQuery { }));
        }
        [Authorize(Roles = "Admin,Moderator")]
        [HttpPost("createCategory")]
        public async Task<IActionResult> AddCategory(CategoryDto category) { 

            return HandleResponse(await Mediator.Send(new AddCategoryCommand { category = category }));
        }

        [Authorize(Roles ="Admin,Moderator")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            return HandleResponse(await Mediator.Send(new DeleteCategoryCommand { CategoryId = id }));
        }

    }
}
