using API.Controllers;
using Application.DTOs.Categories;
using Application.Features.Category.Commands.AddCategory;
using Application.Features.Category.Queries.GetAllCategories;
using MediatR;
using Moq;
using NUnit.Framework;
namespace BarterlyUnitTests.ControllerTests.CategoryControllerTests
{
    public class CategoryControllerTests
    {
        private Mock<IMediator> _mediatorMock;
        private CategoryController _controller;

        [SetUp]
        public void SetUp()
        {
            _mediatorMock = new Mock<IMediator>();

            _controller = new CategoryController();
        }

        [Test]
        public async Task GetAllCategories_ReturnsCategories()
        {
            // Arrange
            var categories = new List<CategoryDto> { new CategoryDto { Id = Guid.NewGuid(), Name = "Category 1" } };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllCategoriesQuery>(), default))
                         .ReturnsAsync(categories);

            // Act
            var result = await _controller.GetAllCategories();

            // Assert
            var okResult = Xunit.Assert.IsType<Task<ICollection<CategoryDto>>>(result);
            var resultValue = await okResult;
            NUnit.Framework.Assert.Equals(1, resultValue.Count);
            NUnit.Framework.Assert.Equals("Category 1", resultValue.First().Name);
        }

        [Test]
        public async Task AddCategory_CallsMediatorWithCorrectCommand()
        {
            // Arrange
            var categoryDto = new CategoryDto { Name = "New Category" };

            // Act
            await _controller.AddCategory(categoryDto);

            // Assert
            _mediatorMock.Verify(m => m.Send(It.Is<AddCategoryCommand>(c => c.category.Name == "New Category"), default), Times.Once);
        }
    }
}
