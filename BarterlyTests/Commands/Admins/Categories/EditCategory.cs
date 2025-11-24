using Application.Commands.Admins.Categories.EditCategory;
using Application.Core.MediatR.Requests;
using Application.DTOs.Categories;
using AutoMapper;
using Domain.Entities.Categories;
using Domain.Enums.Users;
using Domain.Interfaces.Commands.Post;
using Moq;

namespace BarterlyUnitTests.Commands.Admins.Categories;

public class EditCategory
{
    private readonly Mock<ICategoryCommandRepository> _categoryCommandRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly EditCategoryCommandHandler _handler;

    public EditCategory()
    {
        _categoryCommandRepositoryMock = new Mock<ICategoryCommandRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new EditCategoryCommandHandler(
            _categoryCommandRepositoryMock.Object,
            _mapperMock.Object
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

    private Category CreateCategory()
    {
        return new Category
        {
            Id = Guid.NewGuid(),
            NameEN = "Test",
            NamePL = "Test"
        };
    }

    [Fact]
    public async Task Handle_WhenValidCommand_UpdatesCategory()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var command = new EditCategoryCommand
        {
            Id = categoryId,
            NameEn = "Electronics Updated",
            NamePl = "Elektronika Zaktualizowana",
            Description = "Updated description",
            SubCategories = new List<SubCategoryDto>(),
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
        };

        Category capturedCategory = null;
        _categoryCommandRepositoryMock
            .Setup(x => x.UpdateCategoryAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
            .Callback<Category, CancellationToken>((cat, ct) => capturedCategory = cat)
            .ReturnsAsync(CreateCategory());

        _mapperMock
            .Setup(x => x.Map<List<SubCategory>>(It.IsAny<List<SubCategoryDto>>()))
            .Returns(new List<SubCategory>());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(capturedCategory);
        Assert.Equal(categoryId, capturedCategory.Id);
        Assert.Equal("Electronics Updated", capturedCategory.NameEN);
        Assert.Equal("Elektronika Zaktualizowana", capturedCategory.NamePL);
        Assert.Equal("Updated description", capturedCategory.Description);
    }

    [Fact]
    public async Task Handle_CallsUpdateCategoryAsyncWithCorrectData()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var command = new EditCategoryCommand
        {
            Id = categoryId,
            NameEn = "Books",
            NamePl = "Książki",
            SubCategories = new List<SubCategoryDto>(),
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
        };

        _categoryCommandRepositoryMock
            .Setup(x => x.UpdateCategoryAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateCategory());

        _mapperMock
            .Setup(x => x.Map<List<SubCategory>>(It.IsAny<List<SubCategoryDto>>()))
            .Returns(new List<SubCategory>());

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _categoryCommandRepositoryMock.Verify(
            x => x.UpdateCategoryAsync(
                It.Is<Category>(c => 
                    c.Id == categoryId && 
                    c.NameEN == "Books" && 
                    c.NamePL == "Książki"),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_MapsSubCategoriesCorrectly()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var subCategoryId1 = Guid.NewGuid();
        var subCategoryId2 = Guid.NewGuid();

        var subCategoryDtos = new List<SubCategoryDto>
        {
            new SubCategoryDto
            {
                Id = subCategoryId1,
                NameEN = "Phones",
                NamePL = "Telefony",
                CategoryId = categoryId
            },
            new SubCategoryDto
            {
                Id = subCategoryId2,
                NameEN = "Laptops",
                NamePL = "Laptopy",
                CategoryId = categoryId
            }
        };

        var command = new EditCategoryCommand
        {
            Id = categoryId,
            NameEn = "Electronics",
            NamePl = "Elektronika",
            SubCategories = subCategoryDtos,
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
        };

        var mappedSubCategories = new List<SubCategory>
        {
            new SubCategory
            {
                Id = subCategoryId1,
                TitleEN = "Phones",
                TitlePL = "Telefony",
                CategoryId = categoryId
            },
            new SubCategory
            {
                Id = subCategoryId2,
                TitleEN = "Laptops",
                TitlePL = "Laptopy",
                CategoryId = categoryId
            }
        };

        _categoryCommandRepositoryMock
            .Setup(x => x.UpdateCategoryAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateCategory());

        _mapperMock
            .Setup(x => x.Map<List<SubCategory>>(subCategoryDtos))
            .Returns(mappedSubCategories);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mapperMock.Verify(
            x => x.Map<List<SubCategory>>(subCategoryDtos),
            Times.Once);
    }

    [Fact]
    public async Task Handle_PassesMappedSubCategoriesToRepository()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var subCategoryDtos = new List<SubCategoryDto>
        {
            new SubCategoryDto
            {
                Id = Guid.NewGuid(),
                NameEN = "Test",
                NamePL = "Test",
                CategoryId = categoryId
            }
        };

        var mappedSubCategories = new List<SubCategory>
        {
            new SubCategory
            {
                Id = Guid.NewGuid(),
                TitleEN = "Test",
                TitlePL = "Test",
                CategoryId = categoryId
            }
        };

        var command = new EditCategoryCommand
        {
            Id = categoryId,
            NameEn = "Electronics",
            NamePl = "Elektronika",
            SubCategories = subCategoryDtos,
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
        };

        Category capturedCategory = null;
        _categoryCommandRepositoryMock
            .Setup(x => x.UpdateCategoryAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
            .Callback<Category, CancellationToken>((cat, ct) => capturedCategory = cat)
            .ReturnsAsync(CreateCategory());

        _mapperMock
            .Setup(x => x.Map<List<SubCategory>>(subCategoryDtos))
            .Returns(mappedSubCategories);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(capturedCategory);
        Assert.Equal(mappedSubCategories, capturedCategory.SubCategories);
    }

    [Fact]
    public async Task Handle_WhenDescriptionIsNull_UpdatesWithNullDescription()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var command = new EditCategoryCommand
        {
            Id = categoryId,
            NameEn = "Books",
            NamePl = "Książki",
            Description = null,
            SubCategories = new List<SubCategoryDto>(),
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
        };

        Category capturedCategory = null;
        _categoryCommandRepositoryMock
            .Setup(x => x.UpdateCategoryAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
            .Callback<Category, CancellationToken>((cat, ct) => capturedCategory = cat)
            .ReturnsAsync(CreateCategory());

        _mapperMock
            .Setup(x => x.Map<List<SubCategory>>(It.IsAny<List<SubCategoryDto>>()))
            .Returns(new List<SubCategory>());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(capturedCategory);
        Assert.Null(capturedCategory.Description);
    }

    [Fact]
    public async Task Handle_ReturnsSuccessResponse()
    {
        // Arrange
        var command = new EditCategoryCommand
        {
            Id = Guid.NewGuid(),
            NameEn = "Test",
            NamePl = "Test",
            SubCategories = new List<SubCategoryDto>(),
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
        };

        _categoryCommandRepositoryMock
            .Setup(x => x.UpdateCategoryAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateCategory());

        _mapperMock
            .Setup(x => x.Map<List<SubCategory>>(It.IsAny<List<SubCategoryDto>>()))
            .Returns(new List<SubCategory>());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task Handle_PassesCancellationTokenToRepository()
    {
        // Arrange
        var command = new EditCategoryCommand
        {
            Id = Guid.NewGuid(),
            NameEn = "Test",
            NamePl = "Test",
            SubCategories = new List<SubCategoryDto>(),
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
        };

        var cancellationToken = new CancellationToken();

        _categoryCommandRepositoryMock
            .Setup(x => x.UpdateCategoryAsync(It.IsAny<Category>(), cancellationToken))
            .ReturnsAsync(CreateCategory());

        _mapperMock
            .Setup(x => x.Map<List<SubCategory>>(It.IsAny<List<SubCategoryDto>>()))
            .Returns(new List<SubCategory>());

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        _categoryCommandRepositoryMock.Verify(
            x => x.UpdateCategoryAsync(It.IsAny<Category>(), cancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithEmptySubCategories_UpdatesCategory()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var command = new EditCategoryCommand
        {
            Id = categoryId,
            NameEn = "Books",
            NamePl = "Książki",
            SubCategories = new List<SubCategoryDto>(),
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
        };

        Category capturedCategory = null;
        _categoryCommandRepositoryMock
            .Setup(x => x.UpdateCategoryAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
            .Callback<Category, CancellationToken>((cat, ct) => capturedCategory = cat)
            .ReturnsAsync(CreateCategory());

        _mapperMock
            .Setup(x => x.Map<List<SubCategory>>(It.IsAny<List<SubCategoryDto>>()))
            .Returns(new List<SubCategory>());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(capturedCategory);
        Assert.Empty(capturedCategory.SubCategories);
    }

    [Fact]
    public async Task Handle_WithMultipleSubCategories_UpdatesAllCorrectly()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var subCategoryDtos = new List<SubCategoryDto>
        {
            new SubCategoryDto { Id = Guid.NewGuid(), NameEN = "Sub1", NamePL = "Pod1", CategoryId = categoryId },
            new SubCategoryDto { Id = Guid.NewGuid(), NameEN = "Sub2", NamePL = "Pod2", CategoryId = categoryId },
            new SubCategoryDto { Id = Guid.NewGuid(), NameEN = "Sub3", NamePL = "Pod3", CategoryId = categoryId }
        };

        var command = new EditCategoryCommand
        {
            Id = categoryId,
            NameEn = "Services",
            NamePl = "Usługi",
            SubCategories = subCategoryDtos,
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
        };

        var mappedSubCategories = new List<SubCategory>
        {
            new SubCategory { Id = subCategoryDtos[0].Id, TitleEN = "Sub1", TitlePL = "Pod1" },
            new SubCategory { Id = subCategoryDtos[1].Id, TitleEN = "Sub2", TitlePL = "Pod2" },
            new SubCategory { Id = subCategoryDtos[2].Id, TitleEN = "Sub3", TitlePL = "Pod3" }
        };

        _categoryCommandRepositoryMock
            .Setup(x => x.UpdateCategoryAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateCategory());

        _mapperMock
            .Setup(x => x.Map<List<SubCategory>>(subCategoryDtos))
            .Returns(mappedSubCategories);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _mapperMock.Verify(
            x => x.Map<List<SubCategory>>(subCategoryDtos),
            Times.Once);
    }

    [Fact]
    public async Task Handle_UpdatesCategoryFieldsCorrectly()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var command = new EditCategoryCommand
        {
            Id = categoryId,
            NameEn = "New Name EN",
            NamePl = "New Name PL",
            Description = "New Description",
            SubCategories = new List<SubCategoryDto>(),
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
        };

        Category capturedCategory = null;
        _categoryCommandRepositoryMock
            .Setup(x => x.UpdateCategoryAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
            .Callback<Category, CancellationToken>((cat, ct) => capturedCategory = cat)
            .ReturnsAsync(CreateCategory());

        _mapperMock
            .Setup(x => x.Map<List<SubCategory>>(It.IsAny<List<SubCategoryDto>>()))
            .Returns(new List<SubCategory>());

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(capturedCategory);
        Assert.Equal(categoryId, capturedCategory.Id);
        Assert.Equal("New Name EN", capturedCategory.NameEN);
        Assert.Equal("New Name PL", capturedCategory.NamePL);
        Assert.Equal("New Description", capturedCategory.Description);
    }
}