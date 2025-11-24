using Application.Queries.Admins.Categories.GetAllCategories;
using Domain.Entities.Categories;
using Domain.Interfaces.Queries.Post;
using Moq;

namespace BarterlyUnitTests.Queries.Admin.Categories;

public class GetAllCategoriesHandlerTests
{
    private readonly Mock<ICategoryQueryRepository> _repoMock;
    private readonly GetAllCategoriesHandler _handler;

    public GetAllCategoriesHandlerTests()
    {
        _repoMock = new Mock<ICategoryQueryRepository>();
        _handler = new GetAllCategoriesHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_InvalidPagination_ReturnsFailure()
    {
        var query = new GetAllCategoriesQuery
        {
            FilterBy = new FilterBy
            {
                PageNumber = 0,
                PageSize = 0
            }
        };

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal("Invalid pagination parameters.", result.Error);
    }

    [Fact]
    public async Task Handle_NoFilters_ReturnsCategoriesWithDefaultSorting()
    {
        var categories = new List<Category>
        {
            new Category { Id = Guid.NewGuid(), NameEN = "B", NamePL = "B", Description = "desc1" },
            new Category { Id = Guid.NewGuid(), NameEN = "A", NamePL = "A", Description = "desc2" },
        }.AsQueryable();

        _repoMock.Setup(r => r.GetCategoriesAsync()).Returns(categories);

        var query = new GetAllCategoriesQuery
        {
            FilterBy = new FilterBy { PageNumber = 1, PageSize = 10 }
        };

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.TotalCount);
        Assert.Equal(1, result.Value.TotalPages);
        Assert.Equal("B", result.Value.Categories.First().NameEN); // Descending by default
    }

    [Fact]
    public async Task Handle_WithSearchFilter_ReturnsFilteredCategories()
    {
        var categories = new List<Category>
        {
            new Category { Id = Guid.NewGuid(), NameEN = "Test", NamePL = "Test", Description = "something" },
            new Category { Id = Guid.NewGuid(), NameEN = "Other", NamePL = "Other", Description = "nothing" },
        }.AsQueryable();

        _repoMock.Setup(r => r.GetCategoriesAsync()).Returns(categories);

        var query = new GetAllCategoriesQuery
        {
            FilterBy = new FilterBy { Search = "Test", PageNumber = 1, PageSize = 10 }
        };

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.Single(result.Value.Categories);
        Assert.Equal("Test", result.Value.Categories.First().NameEN);
    }

    [Fact]
    public async Task Handle_WithSortBy_ReturnsSortedCategories()
    {
        var categories = new List<Category>
        {
            new Category { Id = Guid.NewGuid(), NameEN = "B", NamePL = "B", Description = "desc" },
            new Category { Id = Guid.NewGuid(), NameEN = "A", NamePL = "A", Description = "desc" },
        }.AsQueryable();

        _repoMock.Setup(r => r.GetCategoriesAsync()).Returns(categories);

        var query = new GetAllCategoriesQuery
        {
            FilterBy = new FilterBy { PageNumber = 1, PageSize = 10 },
            SortBy = new SortBy { SortField = "nameEN", IsDescending = false }
        };

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.Equal("A", result.Value.Categories.First().NameEN);
        Assert.Equal("B", result.Value.Categories.Last().NameEN);
    }

    [Fact]
    public async Task Handle_Exception_ReturnsFailure()
    {
        _repoMock.Setup(r => r.GetCategoriesAsync()).Throws(new Exception("Database error"));

        var query = new GetAllCategoriesQuery();

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Contains("Database error", result.Error);
    }
}