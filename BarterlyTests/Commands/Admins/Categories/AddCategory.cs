using Application.Commands.Admins.Categories.AddCategory;
using Application.Core.MediatR.Requests;
using Application.Interfaces;
using Domain.Entities.Categories;
using Domain.Enums.Common;
using Domain.Enums.Users;
using Domain.Interfaces.Commands.Post;
using Moq;

namespace BarterlyUnitTests.Commands.Admins.Categories;

public class AddCategory
{
    private readonly Mock<ICategoryCommandRepository> _categoryCommandRepositoryMock;
    private readonly Mock<ILogService> _logServiceMock;
    private readonly AddCategoryCommandHanlder _handler;

    public AddCategory()
    {
        _categoryCommandRepositoryMock = new Mock<ICategoryCommandRepository>();
        _logServiceMock = new Mock<ILogService>();
        _handler = new AddCategoryCommandHanlder(
            _categoryCommandRepositoryMock.Object,
            _logServiceMock.Object
        );
    }

    private AuthorizeData CreateAuthorizeData(Guid userId)
    {
        return new AuthorizeData(
            userId,
            new List<UserRoles> { UserRoles.Admin },
            "test-token",
            null
        );
    }

    [Fact]
    public async Task Handle_WhenValidCommand_CreatesCategory()
    {
        // Arrange
        var command = new AddCategoryCommand
        {
            NameEN = "Electronics",
            NamePL = "Elektronika",
            Description = "Electronic devices",
            SubCategories = new List<AddCategoryCommand.SubCategory>(),
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
        };

        Category capturedCategory = null;
        _categoryCommandRepositoryMock
            .Setup(x => x.CreateCategoryAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
            .Callback<Category, CancellationToken>((cat, ct) => capturedCategory = cat)
            .ReturnsAsync((Category cat, CancellationToken ct) => cat);

        _logServiceMock
            .Setup(x => x.CreateLogAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<LogType?>(),
                It.IsAny<string?>(),
                It.IsAny<Guid?>(),
                It.IsAny<Guid?>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(201, result.StatusCode);
        Assert.NotNull(capturedCategory);
        Assert.Equal("Electronics", capturedCategory.NameEN);
        Assert.Equal("Elektronika", capturedCategory.NamePL);
        Assert.Equal("Electronic devices", capturedCategory.Description);
    }

    [Fact]
    public async Task Handle_AlwaysAddsDefaultSubCategory()
    {
        // Arrange
        var command = new AddCategoryCommand
        {
            NameEN = "Books",
            NamePL = "Książki",
            SubCategories = new List<AddCategoryCommand.SubCategory>(),
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
        };

        Category capturedCategory = null;
        _categoryCommandRepositoryMock
            .Setup(x => x.CreateCategoryAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
            .Callback<Category, CancellationToken>((cat, ct) => capturedCategory = cat)
            .ReturnsAsync((Category cat, CancellationToken ct) => cat);
        
        _logServiceMock
            .Setup(x => x.CreateLogAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<LogType?>(),
                It.IsAny<string?>(),
                It.IsAny<Guid?>(),
                It.IsAny<Guid?>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(capturedCategory);
        Assert.Single(capturedCategory.SubCategories);
        
        var defaultSubCategory = capturedCategory.SubCategories.First();
        Assert.Equal("All", defaultSubCategory.TitleEN);
        Assert.Equal("Wszystkie", defaultSubCategory.TitlePL);
        Assert.Equal(capturedCategory.Id, defaultSubCategory.CategoryId);
    }

    [Fact]
    public async Task Handle_WhenSubCategoriesProvided_AddsAllSubCategories()
    {
        // Arrange
        var command = new AddCategoryCommand
        {
            NameEN = "Electronics",
            NamePL = "Elektronika",
            SubCategories = new List<AddCategoryCommand.SubCategory>
            {
                new AddCategoryCommand.SubCategory("Phones", "Telefony"),
                new AddCategoryCommand.SubCategory("Laptops", "Laptopy")
            },
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
        };

        Category capturedCategory = null;
        _categoryCommandRepositoryMock
            .Setup(x => x.CreateCategoryAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
            .Callback<Category, CancellationToken>((cat, ct) => capturedCategory = cat)
            .ReturnsAsync((Category cat, CancellationToken ct) => cat);

        _logServiceMock
            .Setup(x => x.CreateLogAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<LogType?>(),
                It.IsAny<string?>(),
                It.IsAny<Guid?>(),
                It.IsAny<Guid?>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(capturedCategory);
        Assert.Equal(3, capturedCategory.SubCategories.Count); // Default + 2 custom
        
        // Check default subcategory
        Assert.Contains(capturedCategory.SubCategories, sc => sc.TitleEN == "All" && sc.TitlePL == "Wszystkie");
        
        // Check custom subcategories
        Assert.Contains(capturedCategory.SubCategories, sc => sc.TitleEN == "Phones" && sc.TitlePL == "Telefony");
        Assert.Contains(capturedCategory.SubCategories, sc => sc.TitleEN == "Laptops" && sc.TitlePL == "Laptopy");
    }

    [Fact]
    public async Task Handle_AllSubCategoriesHaveCorrectCategoryId()
    {
        // Arrange
        var command = new AddCategoryCommand
        {
            NameEN = "Electronics",
            NamePL = "Elektronika",
            SubCategories = new List<AddCategoryCommand.SubCategory>
            {
                new AddCategoryCommand.SubCategory("Phones", "Telefony"),
                new AddCategoryCommand.SubCategory("Laptops", "Laptopy")
            },
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
        };

        Category capturedCategory = null;
        _categoryCommandRepositoryMock
            .Setup(x => x.CreateCategoryAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
            .Callback<Category, CancellationToken>((cat, ct) => capturedCategory = cat)
            .ReturnsAsync((Category cat, CancellationToken ct) => cat);

        _logServiceMock
            .Setup(x => x.CreateLogAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<LogType?>(),
                It.IsAny<string?>(),
                It.IsAny<Guid?>(),
                It.IsAny<Guid?>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(capturedCategory);
        Assert.All(capturedCategory.SubCategories, sc => Assert.Equal(capturedCategory.Id, sc.CategoryId));
    }

    [Fact]
    public async Task Handle_CallsRepositoryCreateCategoryAsync()
    {
        // Arrange
        var command = new AddCategoryCommand
        {
            NameEN = "Books",
            NamePL = "Książki",
            SubCategories = new List<AddCategoryCommand.SubCategory>(),
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
        };

        _categoryCommandRepositoryMock
            .Setup(x => x.CreateCategoryAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Category cat, CancellationToken ct) => cat);

        _logServiceMock
            .Setup(x => x.CreateLogAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<LogType?>(),
                It.IsAny<string?>(),
                It.IsAny<Guid?>(),
                It.IsAny<Guid?>()))
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _categoryCommandRepositoryMock.Verify(
            x => x.CreateCategoryAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_CreatesLogWithCorrectInformation()
    {
        // Arrange
        var command = new AddCategoryCommand
        {
            NameEN = "Electronics",
            NamePL = "Elektronika",
            SubCategories = new List<AddCategoryCommand.SubCategory>(),
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
        };

        Category capturedCategory = null;
        _categoryCommandRepositoryMock
            .Setup(x => x.CreateCategoryAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
            .Callback<Category, CancellationToken>((cat, ct) => capturedCategory = cat)
            .ReturnsAsync((Category cat, CancellationToken ct) => cat);

        _logServiceMock
            .Setup(x => x.CreateLogAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<LogType?>(),
                It.IsAny<string?>(),
                It.IsAny<Guid?>(),
                It.IsAny<Guid?>()))
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _logServiceMock.Verify(
            x => x.CreateLogAsync(
                It.Is<string>(msg => msg.Contains(capturedCategory.Id.ToString()) && msg.Contains("Electronics")),
                It.IsAny<CancellationToken>(),
                LogType.Information,
                It.IsAny<string?>(),
                It.IsAny<Guid?>(),
                It.IsAny<Guid?>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenDescriptionIsNull_CreatesCategory()
    {
        // Arrange
        var command = new AddCategoryCommand
        {
            NameEN = "Books",
            NamePL = "Książki",
            Description = null,
            SubCategories = new List<AddCategoryCommand.SubCategory>(),
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
        };

        Category capturedCategory = null;
        _categoryCommandRepositoryMock
            .Setup(x => x.CreateCategoryAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
            .Callback<Category, CancellationToken>((cat, ct) => capturedCategory = cat)
            .ReturnsAsync((Category cat, CancellationToken ct) => cat);

        _logServiceMock
            .Setup(x => x.CreateLogAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<LogType?>(),
                It.IsAny<string?>(),
                It.IsAny<Guid?>(),
                It.IsAny<Guid?>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(capturedCategory);
        Assert.Null(capturedCategory.Description);
    }

    [Fact]
    public async Task Handle_Returns201StatusCode()
    {
        // Arrange
        var command = new AddCategoryCommand
        {
            NameEN = "Books",
            NamePL = "Książki",
            SubCategories = new List<AddCategoryCommand.SubCategory>(),
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
        };

        _categoryCommandRepositoryMock
            .Setup(x => x.CreateCategoryAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Category cat, CancellationToken ct) => cat);

        _logServiceMock
            .Setup(x => x.CreateLogAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<LogType?>(),
                It.IsAny<string?>(),
                It.IsAny<Guid?>(),
                It.IsAny<Guid?>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(201, result.StatusCode);
    }

    [Fact]
    public async Task Handle_WithMultipleSubCategories_AddsAllCorrectly()
    {
        // Arrange
        var command = new AddCategoryCommand
        {
            NameEN = "Services",
            NamePL = "Usługi",
            SubCategories = new List<AddCategoryCommand.SubCategory>
            {
                new AddCategoryCommand.SubCategory("Plumbing", "Hydraulika"),
                new AddCategoryCommand.SubCategory("Electrical", "Elektryka"),
                new AddCategoryCommand.SubCategory("Carpentry", "Stolarstwo")
            },
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
        };

        Category capturedCategory = null;
        _categoryCommandRepositoryMock
            .Setup(x => x.CreateCategoryAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
            .Callback<Category, CancellationToken>((cat, ct) => capturedCategory = cat)
            .ReturnsAsync((Category cat, CancellationToken ct) => cat);

        _logServiceMock
            .Setup(x => x.CreateLogAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<LogType?>(),
                It.IsAny<string?>(),
                It.IsAny<Guid?>(),
                It.IsAny<Guid?>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(capturedCategory);
        Assert.Equal(4, capturedCategory.SubCategories.Count); // Default + 3 custom
    }

    [Fact]
    public async Task Handle_PassesCancellationTokenToRepository()
    {
        // Arrange
        var command = new AddCategoryCommand
        {
            NameEN = "Books",
            NamePL = "Książki",
            SubCategories = new List<AddCategoryCommand.SubCategory>(),
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
        };

        var cancellationToken = CancellationToken.None;

        _categoryCommandRepositoryMock
            .Setup(x => x.CreateCategoryAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Category cat, CancellationToken ct) => cat);

        _logServiceMock
            .Setup(x => x.CreateLogAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<LogType?>(),
                It.IsAny<string?>(),
                It.IsAny<Guid?>(),
                It.IsAny<Guid?>()))
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        _categoryCommandRepositoryMock.Verify(
            x => x.CreateCategoryAsync(It.IsAny<Category>(), cancellationToken),
            Times.Once);
        _logServiceMock
            .Setup(x => x.CreateLogAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<LogType?>(),
                It.IsAny<string?>(),
                It.IsAny<Guid?>(),
                It.IsAny<Guid?>()))
            .Returns(Task.CompletedTask);
    }
}