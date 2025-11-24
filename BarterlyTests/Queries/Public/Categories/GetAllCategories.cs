using Application.Queries.Public.Categories.GetAllCategories;
using Domain.Entities.Categories;
using Domain.Interfaces.Queries.Post;
using MockQueryable;
using Moq;

namespace BarterlyUnitTests.Queries.Public.Categories;

public class GetAllCategoriesHandlerTests
{
    private readonly Mock<ICategoryQueryRepository> _categoryQueryRepositoryMock;
    private readonly GetAllCategoriesHandler _handler;

    public GetAllCategoriesHandlerTests()
    {
        _categoryQueryRepositoryMock = new Mock<ICategoryQueryRepository>();
        _handler = new GetAllCategoriesHandler(_categoryQueryRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WhenCategoriesExist_ReturnsSuccessWithAllCategories()
    {
        // Arrange
        var categoryId1 = Guid.NewGuid();
        var categoryId2 = Guid.NewGuid();
        var subCategoryId1 = Guid.NewGuid();
        var subCategoryId2 = Guid.NewGuid();
        var subCategoryId3 = Guid.NewGuid();

        var categories = new List<Category>
        {
            new Category
            {
                Id = categoryId1,
                NameEN = "Electronics",
                NamePL = "Elektronika",
                Description = "Electronic devices",
                SubCategories = new List<SubCategory>
                {
                    new SubCategory
                    {
                        Id = subCategoryId1,
                        TitleEN = "Phones",
                        TitlePL = "Telefony",
                        CategoryId = categoryId1
                    },
                    new SubCategory
                    {
                        Id = subCategoryId2,
                        TitleEN = "Laptops",
                        TitlePL = "Laptopy",
                        CategoryId = categoryId1
                    }
                }
            },
            new Category
            {
                Id = categoryId2,
                NameEN = "Books",
                NamePL = "Książki",
                Description = "All kinds of books",
                SubCategories = new List<SubCategory>
                {
                    new SubCategory
                    {
                        Id = subCategoryId3,
                        TitleEN = "Fiction",
                        TitlePL = "Fikcja",
                        CategoryId = categoryId2
                    }
                }
            }
        };

        var mockCategories = categories.AsQueryable().BuildMock();

        _categoryQueryRepositoryMock
            .Setup(x => x.GetCategoriesAsync())
            .Returns(mockCategories);

        var query = new GetAllCategoriesQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        
        var resultList = result.Value.ToList();
        Assert.Equal(2, resultList.Count);

        // Verify first category
        var firstCategory = resultList.First(c => c.Id == categoryId1);
        Assert.Equal("Electronics", firstCategory.NameEN);
        Assert.Equal("Elektronika", firstCategory.NamePL);
        Assert.Equal("Electronic devices", firstCategory.Description);
        Assert.Equal(2, firstCategory.SubCategories.Count());

        // Verify second category
        var secondCategory = resultList.First(c => c.Id == categoryId2);
        Assert.Equal("Books", secondCategory.NameEN);
        Assert.Equal("Książki", secondCategory.NamePL);
        Assert.Equal("All kinds of books", secondCategory.Description);
        Assert.Single(secondCategory.SubCategories);

        _categoryQueryRepositoryMock.Verify(x => x.GetCategoriesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenSubCategoriesExist_MapsAllSubCategories()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var subCategoryId1 = Guid.NewGuid();
        var subCategoryId2 = Guid.NewGuid();

        var categories = new List<Category>
        {
            new Category
            {
                Id = categoryId,
                NameEN = "Test Category",
                NamePL = "Kategoria Testowa",
                Description = "Test",
                SubCategories = new List<SubCategory>
                {
                    new SubCategory
                    {
                        Id = subCategoryId1,
                        TitleEN = "SubCat 1 EN",
                        TitlePL = "SubCat 1 PL",
                        CategoryId = categoryId
                    },
                    new SubCategory
                    {
                        Id = subCategoryId2,
                        TitleEN = "SubCat 2 EN",
                        TitlePL = "SubCat 2 PL",
                        CategoryId = categoryId
                    }
                }
            }
        };

        var mockCategories = categories.AsQueryable().BuildMock();

        _categoryQueryRepositoryMock
            .Setup(x => x.GetCategoriesAsync())
            .Returns(mockCategories);

        var query = new GetAllCategoriesQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        var category = result.Value.First();
        var subCategories = category.SubCategories.ToList();

        Assert.Equal(2, subCategories.Count);

        var subCat1 = subCategories.First(s => s.Id == subCategoryId1);
        Assert.Equal("SubCat 1 EN", subCat1.NameEN);
        Assert.Equal("SubCat 1 PL", subCat1.NamePL);

        var subCat2 = subCategories.First(s => s.Id == subCategoryId2);
        Assert.Equal("SubCat 2 EN", subCat2.NameEN);
        Assert.Equal("SubCat 2 PL", subCat2.NamePL);
    }

    [Fact]
    public async Task Handle_WhenCategoryHasNullDescription_ReturnsEmptyString()
    {
        // Arrange
        var categoryId = Guid.NewGuid();

        var categories = new List<Category>
        {
            new Category
            {
                Id = categoryId,
                NameEN = "Test Category",
                NamePL = "Kategoria Testowa",
                Description = null,
                SubCategories = new List<SubCategory>()
            }
        };

        var mockCategories = categories.AsQueryable().BuildMock();

        _categoryQueryRepositoryMock
            .Setup(x => x.GetCategoriesAsync())
            .Returns(mockCategories);

        var query = new GetAllCategoriesQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        var category = result.Value.First();
        Assert.Equal("", category.Description);
    }

    [Fact]
    public async Task Handle_WhenCategoryHasNoSubCategories_ReturnsEmptySubCategoriesList()
    {
        // Arrange
        var categoryId = Guid.NewGuid();

        var categories = new List<Category>
        {
            new Category
            {
                Id = categoryId,
                NameEN = "Empty Category",
                NamePL = "Pusta Kategoria",
                Description = "No subcategories",
                SubCategories = new List<SubCategory>()
            }
        };

        var mockCategories = categories.AsQueryable().BuildMock();

        _categoryQueryRepositoryMock
            .Setup(x => x.GetCategoriesAsync())
            .Returns(mockCategories);

        var query = new GetAllCategoriesQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        var category = result.Value.First();
        Assert.Empty(category.SubCategories);
    }

    [Fact]
    public async Task Handle_WhenNoCategoriesExist_ReturnsEmptyCollection()
    {
        // Arrange
        var emptyCategories = new List<Category>();
        var mockCategories = emptyCategories.AsQueryable().BuildMock();

        _categoryQueryRepositoryMock
            .Setup(x => x.GetCategoriesAsync())
            .Returns(mockCategories);

        var query = new GetAllCategoriesQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Empty(result.Value);

        _categoryQueryRepositoryMock.Verify(x => x.GetCategoriesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_MapsAllCategoryFieldsCorrectly()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var expectedNameEN = "Test Name EN";
        var expectedNamePL = "Test Name PL";
        var expectedDescription = "Test Description";

        var categories = new List<Category>
        {
            new Category
            {
                Id = categoryId,
                NameEN = expectedNameEN,
                NamePL = expectedNamePL,
                Description = expectedDescription,
                SubCategories = new List<SubCategory>()
            }
        };

        var mockCategories = categories.AsQueryable().BuildMock();

        _categoryQueryRepositoryMock
            .Setup(x => x.GetCategoriesAsync())
            .Returns(mockCategories);

        var query = new GetAllCategoriesQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        var category = result.Value.First();
        
        Assert.Equal(categoryId, category.Id);
        Assert.Equal(expectedNameEN, category.NameEN);
        Assert.Equal(expectedNamePL, category.NamePL);
        Assert.Equal(expectedDescription, category.Description);
    }

    [Fact]
    public async Task Handle_WithMultipleCategoriesAndSubCategories_ReturnsCorrectStructure()
    {
        // Arrange
        var categories = new List<Category>();
        
        for (int i = 1; i <= 3; i++)
        {
            var categoryId = Guid.NewGuid();
            var subCategories = new List<SubCategory>();
            
            for (int j = 1; j <= 2; j++)
            {
                subCategories.Add(new SubCategory
                {
                    Id = Guid.NewGuid(),
                    TitleEN = $"SubCat {i}-{j} EN",
                    TitlePL = $"SubCat {i}-{j} PL",
                    CategoryId = categoryId
                });
            }

            categories.Add(new Category()
            {
                Id = categoryId,
                NameEN = $"Category {i} EN",
                NamePL = $"Category {i} PL",
                Description = $"Description {i}",
                SubCategories = subCategories
            });
        }

        var mockCategories = categories.BuildMock();

        _categoryQueryRepositoryMock
            .Setup(x => x.GetCategoriesAsync())
            .Returns(mockCategories);

        var query = new GetAllCategoriesQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        var resultList = result.Value.ToList();
        
        Assert.Equal(3, resultList.Count);
        Assert.All(resultList, category => Assert.Equal(2, category.SubCategories.Count()));
    }
    
}