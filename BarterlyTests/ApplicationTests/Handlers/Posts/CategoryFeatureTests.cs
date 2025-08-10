using Moq;

namespace BarterlyUnitTests.ApplicationTests.Handlers.Posts;

public class CategoryFeatureTests
{
    [Fact]
    public async Task AddCategoryHandle_ShouldReturnSuccess_WhenServiceSucceeds()
    {
        // Arrange
        var mockService = new Mock<ICategoryService>();
        mockService.Setup(s => s.AddCategory(It.IsAny<CategoryDto>()))
            .Returns(Task.CompletedTask);

        var handler = new AddCategoryCommandHanlder(mockService.Object);
        var command = new AddCategoryCommand { category = new CategoryDto() };

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        Assert.True(result.IsSuccess);
        mockService.Verify(s => s.AddCategory(command.category), Times.Once);
    }

    [Fact]
    public async Task AddCategoryHandle_ShouldReturnFailure_WhenServiceThrowsException()
    {
        // Arrange
        var mockService = new Mock<ICategoryService>();
        var message = "Error while creating category";
        mockService.Setup(s => s.AddCategory(It.IsAny<CategoryDto>())).ThrowsAsync(new Exception(message));
        var handler = new AddCategoryCommandHanlder(mockService.Object);
        var command = new AddCategoryCommand { category = new CategoryDto() };

        // Act
        var result = await handler.Handle(command, default);

        //Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(message, result.Error);
        mockService.Verify(s => s.AddCategory(command.category), Times.Once);
    }

    [Fact]
    public async Task DeleteCategoryHandle_ShouldReturnSuccess_WhenServiceSuccessed_WithRightGuid()
    {
        // Arrange
        var mockService = new Mock<ICategoryService>();
        mockService.Setup(s => s.DeleteCategory(It.IsAny<Guid>())).Returns(Task.CompletedTask);

        var handler = new DeleteCategoryCommandHandler(mockService.Object);
        var command = new DeleteCategoryCommand { CategoryId = Guid.NewGuid().ToString() };

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        Assert.True(result.IsSuccess);
        mockService.Verify(s => s.DeleteCategory(Guid.Parse(command.CategoryId)), Times.Once);
    }

    [Fact]
    public async Task DeleteCategoryHandle_ShouldReturnFailure_WhenServiceFailed_WithWrongGuid()
    {
        // Arrange
        var mockService = new Mock<ICategoryService>();
        mockService.Setup(s => s.DeleteCategory(It.IsAny<Guid>())).Returns(Task.CompletedTask);

        var handler = new DeleteCategoryCommandHandler(mockService.Object);
        var command = new DeleteCategoryCommand { CategoryId = "1221" };

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task DeleteCategoryHandle_ShouldReturnFailure_WhenServiceThrowsException()
    {
        // Arrange
        var mockService = new Mock<ICategoryService>();
        var message = "Error while deleting category";
        mockService.Setup(s => s.DeleteCategory(It.IsAny<Guid>())).ThrowsAsync(new Exception(message));

        var handler = new DeleteCategoryCommandHandler(mockService.Object);
        var command = new DeleteCategoryCommand { CategoryId = Guid.NewGuid().ToString() };

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Error while deleting category", result.Error);
    }

    [Fact]
    public async Task EditCategoryHandle_ShouldReturnSuccess_WhenServiceSuccessed()
    {
        // Arrange
        var mockService = new Mock<ICategoryService>();
        mockService.Setup(s => s.EditCategory(It.IsAny<CategoryDto>())).Returns(Task.CompletedTask);

        var handler = new EditCategoryCommandHandler(mockService.Object);
        var command = new EditCategoryCommand { category = new CategoryDto() };

        // Act
        var result = await handler.Handle(command, default);

        // Assert 
        Assert.True(result.IsSuccess);
        mockService.Verify(s => s.EditCategory(command.category), Times.Once);
    }

    [Fact]
    public async Task EditCategoryHandle_ShouldReturnFailure_WhenServiceFailed()
    {
        // Arrange
        var mockService = new Mock<ICategoryService>();
        var message = "Error while editing category";
        mockService.Setup(s => s.EditCategory(It.IsAny<CategoryDto>())).ThrowsAsync(new Exception(message));

        var handler = new EditCategoryCommandHandler(mockService.Object);
        var command = new EditCategoryCommand { category = new CategoryDto() };

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(message, result.Error);
    }

    [Fact]
    public async Task GetAllCategoriesHanlde_ShouldReturnSuccess_WhenServiceSuccessed()
    {
        // Arrange
        var mockService = new Mock<ICategoryService>();
        var categories = new List<CategoryDto>
        {
            new CategoryDto
                { Id = Guid.NewGuid(), NameEN = "Category 1", NamePL = "Kategoria 1", Description = "" },
            new CategoryDto { Id = Guid.NewGuid(), NameEN = "Category 2", NamePL = "Kategoria 1", Description = "" }
        };
        mockService.Setup(s => s.GetAllCategories()).ReturnsAsync(categories);

        var query = new GetAllCategoriesQuery();
        var handler = new GetAllCategoriesHandler(mockService.Object);

        // Act
        var result = await handler.Handle(query, default);

        // Assert

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(categories, result.Value);
        mockService.Verify(s => s.GetAllCategories(), Times.Once());
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenServiceThrowsException()
    {
        // Arrange
        var mockService = new Mock<ICategoryService>();
        var message = "Error while loading categories";
        mockService.Setup(s => s.GetAllCategories()).ThrowsAsync(new Exception(message));

        var handler = new GetAllCategoriesHandler(mockService.Object);
        var query = new GetAllCategoriesQuery();

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(message, result.Error);
    }
}