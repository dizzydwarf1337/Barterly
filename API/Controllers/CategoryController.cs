using Application.DTOs;
using Application.DTOs.Categories;
using Application.Features.Category.Commands.AddCategory;
using Application.Features.Category.Commands.DeleteCategory;
using Application.Features.Category.Commands.EditCategory;
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

            return HandleResponse(await Mediator.Send(new GetAllCategoriesQuery { }));
        }

        [Authorize(Policy = "Admin")]
        [Authorize(Policy = "Moderator")]
        [HttpPost("createCategory")]
        public async Task<IActionResult> AddCategory(CategoryDto category)
        {
            Console.WriteLine(category.ToString());
            return HandleResponse(await Mediator.Send(new AddCategoryCommand { category = category }));
        }


        [Authorize(Policy = "Admin")]
        [Authorize(Policy = "Moderator")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            return HandleResponse(await Mediator.Send(new DeleteCategoryCommand { CategoryId = id }));
        }


        [Authorize(Policy = "Admin")]
        [Authorize(Policy = "Moderator")]
        [HttpPut("update")]
        public async Task<IActionResult> EditCategory(CategoryDto category)
        {
            return HandleResponse(await Mediator.Send(new EditCategoryCommand { category = category }));
        }

    }
}
