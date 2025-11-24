using Application.DTOs.Posts;
using Application.Queries.Public.Posts.GetPostsFiltredPaginated;
using AutoMapper;
using Domain.Entities.Categories;
using Domain.Entities.Posts;
using Domain.Entities.Posts.PostTypes;
using Domain.Enums.Posts;
using Domain.Interfaces.Queries.Post;
using MockQueryable;
using Moq;

namespace BarterlyUnitTests.Queries.Public.Posts;

public class GetPostsQueryHandlerTests
{
    private readonly Mock<IPostQueryRepository> _postQueryRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetPostsQueryHandler _handler;

    public GetPostsQueryHandlerTests()
    {
        _postQueryRepositoryMock = new Mock<IPostQueryRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetPostsQueryHandler(
            _postQueryRepositoryMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_WhenValidRequest_ReturnsPaginatedResults()
    {
        // Arrange
        var posts = CreateCommonPosts(15);
        var mockPosts = posts.AsQueryable().BuildMock();

        _postQueryRepositoryMock
            .Setup(x => x.GetAllPosts())
            .Returns(mockPosts);

        _mapperMock
            .Setup(x => x.Map<List<PostPreviewDto>>(It.IsAny<List<Post>>()))
            .Returns((List<Post> p) => p.Select(post => new PostPreviewDto
            {
                Id = post.Id.ToString(),
                SubCategoryId = post.SubCategoryId.ToString()
            }).ToList());

        var query = new GetPostsQuery
        {
            FilterBy = new GetPostsQuery.FilterSpecification
            {
                PageSize = 10,
                PageNumber = 1
            }
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(15, result.Value.TotalCount);
        Assert.Equal(2, result.Value.TotalPages);
        Assert.Equal(10, result.Value.Items.Count);
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(-1, 1)]
    [InlineData(10, 0)]
    [InlineData(10, -1)]
    public async Task Handle_WhenInvalidPaginationParameters_ReturnsFailure(int pageSize, int pageNumber)
    {
        // Arrange
        var query = new GetPostsQuery
        {
            FilterBy = new GetPostsQuery.FilterSpecification
            {
                PageSize = pageSize,
                PageNumber = pageNumber
            }
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Invalid pagination parameters.", result.Error);
    }

    [Fact]
    public async Task Handle_WhenSearchFilterApplied_ReturnsMatchingPosts()
    {
        // Arrange
        var posts = new List<Post>
        {
            CreateCommonPost("Laptop for sale"),
            CreateCommonPost("Phone for sale"),
            CreateCommonPost("Book collection")
        };
        var mockPosts = posts.AsQueryable().BuildMock();

        _postQueryRepositoryMock
            .Setup(x => x.GetAllPosts())
            .Returns(mockPosts);

        _mapperMock
            .Setup(x => x.Map<List<PostPreviewDto>>(It.IsAny<List<Post>>()))
            .Returns((List<Post> p) => p.Select(post => new PostPreviewDto
            {
                Id = post.Id.ToString(),
                Title = post.Title,
                SubCategoryId = post.SubCategoryId.ToString()
            }).ToList());

        var query = new GetPostsQuery
        {
            FilterBy = new GetPostsQuery.FilterSpecification
            {
                Search = "sale",
                PageSize = 10,
                PageNumber = 1
            }
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.TotalCount);
    }

    [Fact]
    public async Task Handle_WhenCategoryIdFilterApplied_ReturnsMatchingPosts()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var posts = new List<Post>
        {
            CreateCommonPostWithCategory(categoryId),
            CreateCommonPostWithCategory(Guid.NewGuid()),
            CreateCommonPostWithCategory(categoryId)
        };
        var mockPosts = posts.AsQueryable().BuildMock();

        _postQueryRepositoryMock
            .Setup(x => x.GetAllPosts())
            .Returns(mockPosts);

        _mapperMock
            .Setup(x => x.Map<List<PostPreviewDto>>(It.IsAny<List<Post>>()))
            .Returns(new List<PostPreviewDto>());

        var query = new GetPostsQuery
        {
            FilterBy = new GetPostsQuery.FilterSpecification
            {
                CategoryId = categoryId,
                PageSize = 10,
                PageNumber = 1
            }
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.TotalCount);
    }

    [Fact]
    public async Task Handle_WhenSubCategoryIdFilterApplied_ReturnsMatchingPosts()
    {
        // Arrange
        var subCategoryId = Guid.NewGuid();
        var posts = new List<Post>
        {
            CreateCommonPostWithSubCategory(subCategoryId),
            CreateCommonPostWithSubCategory(Guid.NewGuid()),
            CreateCommonPostWithSubCategory(subCategoryId)
        };
        var mockPosts = posts.AsQueryable().BuildMock();

        _postQueryRepositoryMock
            .Setup(x => x.GetAllPosts())
            .Returns(mockPosts);

        _mapperMock
            .Setup(x => x.Map<List<PostPreviewDto>>(It.IsAny<List<Post>>()))
            .Returns(new List<PostPreviewDto>());

        var query = new GetPostsQuery
        {
            FilterBy = new GetPostsQuery.FilterSpecification
            {
                SubCategoryId = subCategoryId,
                PageSize = 10,
                PageNumber = 1
            }
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.TotalCount);
    }

    [Fact]
    public async Task Handle_WhenCityFilterApplied_ReturnsMatchingPosts()
    {
        // Arrange
        var posts = new List<Post>
        {
            CreateCommonPostWithCity("Warsaw"),
            CreateCommonPostWithCity("Krakow"),
            CreateCommonPostWithCity("Warsaw")
        };
        var mockPosts = posts.AsQueryable().BuildMock();

        _postQueryRepositoryMock
            .Setup(x => x.GetAllPosts())
            .Returns(mockPosts);

        _mapperMock
            .Setup(x => x.Map<List<PostPreviewDto>>(It.IsAny<List<Post>>()))
            .Returns(new List<PostPreviewDto>());

        var query = new GetPostsQuery
        {
            FilterBy = new GetPostsQuery.FilterSpecification
            {
                City = "Warsaw",
                PageSize = 10,
                PageNumber = 1
            }
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.TotalCount);
    }

    [Fact]
    public async Task Handle_WhenPriceRangeFilterApplied_ReturnsMatchingPosts()
    {
        // Arrange
        var posts = new List<Post>
        {
            CreateCommonPostWithPrice(100m),
            CreateCommonPostWithPrice(500m),
            CreateCommonPostWithPrice(1000m)
        };
        var mockPosts = posts.AsQueryable().BuildMock();

        _postQueryRepositoryMock
            .Setup(x => x.GetAllPosts())
            .Returns(mockPosts);

        _mapperMock
            .Setup(x => x.Map<List<PostPreviewDto>>(It.IsAny<List<Post>>()))
            .Returns(new List<PostPreviewDto>());

        var query = new GetPostsQuery
        {
            FilterBy = new GetPostsQuery.FilterSpecification
            {
                MinPrice = 200m,
                MaxPrice = 800m,
                PageSize = 10,
                PageNumber = 1
            }
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Value.TotalCount);
    }

    [Fact]
    public async Task Handle_WhenWorkPostFiltersApplied_ReturnsMatchingPosts()
    {
        // Arrange
        var posts = new List<Post>
        {
            CreateWorkPost(WorkloadType.FullTime, WorkLocationType.Remote, 5000m, 8000m, true),
            CreateWorkPost(WorkloadType.PartTime, WorkLocationType.OnSite, 3000m, 5000m, false),
            CreateWorkPost(WorkloadType.FullTime, WorkLocationType.Hybrid, 6000m, 10000m, true)
        };
        var mockPosts = posts.AsQueryable().BuildMock();

        _postQueryRepositoryMock
            .Setup(x => x.GetAllPosts())
            .Returns(mockPosts);

        _mapperMock
            .Setup(x => x.Map<List<PostPreviewDto>>(It.IsAny<List<Post>>()))
            .Returns(new List<PostPreviewDto>());

        var query = new GetPostsQuery
        {
            FilterBy = new GetPostsQuery.FilterSpecification
            {
                PostType = "Work",
                Workload = WorkloadType.FullTime,
                ExperienceRequired = true,
                PageSize = 10,
                PageNumber = 1
            }
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.TotalCount);
    }

    [Fact]
    public async Task Handle_WhenRentPostFiltersApplied_ReturnsMatchingPosts()
    {
        // Arrange
        var posts = new List<Post>
        {
            CreateRentPost(RentObjectType.Apartment, 3, 75m, 5),
            CreateRentPost(RentObjectType.House, 5, 150m, 1),
            CreateRentPost(RentObjectType.Apartment, 2, 50m, 3)
        };
        var mockPosts = posts.AsQueryable().BuildMock();

        _postQueryRepositoryMock
            .Setup(x => x.GetAllPosts())
            .Returns(mockPosts);

        _mapperMock
            .Setup(x => x.Map<List<PostPreviewDto>>(It.IsAny<List<Post>>()))
            .Returns(new List<PostPreviewDto>());

        var query = new GetPostsQuery
        {
            FilterBy = new GetPostsQuery.FilterSpecification
            {
                PostType = "Rent",
                RentObjectType = RentObjectType.Apartment,
                NumberOfRooms = 3,
                PageSize = 10,
                PageNumber = 1
            }
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Value.TotalCount);
    }

    [Theory]
    [InlineData("title", false)]
    [InlineData("title", true)]
    [InlineData("createdat", false)]
    [InlineData("createdat", true)]
    public async Task Handle_WhenSortByApplied_ReturnsSortedPosts(string sortBy, bool isDescending)
    {
        // Arrange
        var posts = CreateCommonPosts(5);
        var mockPosts = posts.AsQueryable().BuildMock();

        _postQueryRepositoryMock
            .Setup(x => x.GetAllPosts())
            .Returns(mockPosts);

        _mapperMock
            .Setup(x => x.Map<List<PostPreviewDto>>(It.IsAny<List<Post>>()))
            .Returns((List<Post> p) => p.Select(post => new PostPreviewDto
            {
                Id = post.Id.ToString(),
                SubCategoryId = post.SubCategoryId.ToString()
            }).ToList());

        var query = new GetPostsQuery
        {
            FilterBy = new GetPostsQuery.FilterSpecification
            {
                PageSize = 10,
                PageNumber = 1
            },
            SortBy = new GetPostsQuery.SortSpecification
            {
                SortBy = sortBy,
                IsDescending = isDescending
            }
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(5, result.Value.Items.Count);
    }

    [Fact]
    public async Task Handle_ExcludesDeletedPosts()
    {
        // Arrange
        var posts = new List<Post>
        {
            CreateCommonPost("Post 1", isDeleted: false),
            CreateCommonPost("Post 2", isDeleted: true),
            CreateCommonPost("Post 3", isDeleted: false)
        };
        var mockPosts = posts.AsQueryable().BuildMock();

        _postQueryRepositoryMock
            .Setup(x => x.GetAllPosts())
            .Returns(mockPosts);

        _mapperMock
            .Setup(x => x.Map<List<PostPreviewDto>>(It.IsAny<List<Post>>()))
            .Returns(new List<PostPreviewDto>());

        var query = new GetPostsQuery
        {
            FilterBy = new GetPostsQuery.FilterSpecification
            {
                PageSize = 10,
                PageNumber = 1
            }
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.TotalCount);
    }

    [Fact]
    public async Task Handle_ExcludesHiddenPosts()
    {
        // Arrange
        var posts = new List<Post>
        {
            CreateCommonPost("Post 1", isHidden: false),
            CreateCommonPost("Post 2", isHidden: true),
            CreateCommonPost("Post 3", isHidden: false)
        };
        var mockPosts = posts.AsQueryable().BuildMock();

        _postQueryRepositoryMock
            .Setup(x => x.GetAllPosts())
            .Returns(mockPosts);

        _mapperMock
            .Setup(x => x.Map<List<PostPreviewDto>>(It.IsAny<List<Post>>()))
            .Returns(new List<PostPreviewDto>());

        var query = new GetPostsQuery
        {
            FilterBy = new GetPostsQuery.FilterSpecification
            {
                PageSize = 10,
                PageNumber = 1
            }
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.TotalCount);
    }

    [Fact]
    public async Task Handle_OnlyIncludesPublishedPosts()
    {
        // Arrange
        var posts = new List<Post>
        {
            CreateCommonPost("Post 1", status: PostStatusType.Published),
            CreateCommonPost("Post 2", status: PostStatusType.UnderReview),
            CreateCommonPost("Post 3", status: PostStatusType.Published)
        };
        var mockPosts = posts.AsQueryable().BuildMock();

        _postQueryRepositoryMock
            .Setup(x => x.GetAllPosts())
            .Returns(mockPosts);

        _mapperMock
            .Setup(x => x.Map<List<PostPreviewDto>>(It.IsAny<List<Post>>()))
            .Returns(new List<PostPreviewDto>());

        var query = new GetPostsQuery
        {
            FilterBy = new GetPostsQuery.FilterSpecification
            {
                PageSize = 10,
                PageNumber = 1
            }
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.TotalCount);
    }

    [Fact]
    public async Task Handle_WhenNoPostsMatch_ReturnsEmptyResult()
    {
        // Arrange
        var emptyPosts = new List<Post>();
        var mockPosts = emptyPosts.AsQueryable().BuildMock();

        _postQueryRepositoryMock
            .Setup(x => x.GetAllPosts())
            .Returns(mockPosts);

        _mapperMock
            .Setup(x => x.Map<List<PostPreviewDto>>(It.IsAny<List<Post>>()))
            .Returns(new List<PostPreviewDto>());

        var query = new GetPostsQuery
        {
            FilterBy = new GetPostsQuery.FilterSpecification
            {
                PageSize = 10,
                PageNumber = 1
            }
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Value.Items);
        Assert.Equal(0, result.Value.TotalCount);
        Assert.Equal(0, result.Value.TotalPages);
    }

    // Helper methods
    private List<Post> CreateCommonPosts(int count)
    {
        var posts = new List<Post>();
        for (int i = 0; i < count; i++)
        {
            posts.Add(CreateCommonPost($"Post {i + 1}"));
        }
        return posts;
    }

    private Post CreateCommonPost(
        string title = "Test Post",
        bool isDeleted = false,
        bool isHidden = false,
        PostStatusType status = PostStatusType.Published)
    {
        return new CommonPost
        {
            Id = Guid.NewGuid(),
            Title = title,
            FullDescription = "Description",
            ShortDescription = "Short",
            SubCategoryId = Guid.NewGuid(),
            OwnerId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            City = "Warsaw",
            Price = 100m,
            PostSettings = new PostSettings
            {
                Id = Guid.NewGuid(),
                IsDeleted = isDeleted,
                IsHidden = isHidden,
                postStatusType = status
            },
            SubCategory = new SubCategory
            {
                Id = Guid.NewGuid(),
                CategoryId = Guid.NewGuid(),
                TitleEN = "Test SubCategory EN",
                TitlePL = "Test SubCategory PL"
            }
        };
    }

    private Post CreateCommonPostWithCategory(Guid categoryId)
    {
        var post = (CommonPost)CreateCommonPost();
        post.SubCategory.CategoryId = categoryId;
        return post;
    }

    private Post CreateCommonPostWithSubCategory(Guid subCategoryId)
    {
        var post = CreateCommonPost();
        post.SubCategoryId = subCategoryId;
        return post;
    }

    private Post CreateCommonPostWithCity(string city)
    {
        var post = CreateCommonPost();
        post.City = city;
        return post;
    }

    private Post CreateCommonPostWithPrice(decimal price)
    {
        var post = CreateCommonPost();
        post.Price = price;
        return post;
    }

    private WorkPost CreateWorkPost(
        WorkloadType workload,
        WorkLocationType workLocation,
        decimal minSalary,
        decimal maxSalary,
        bool experienceRequired)
    {
        return new WorkPost
        {
            Id = Guid.NewGuid(),
            Title = "Work Post",
            FullDescription = "Description",
            ShortDescription = "Short",
            SubCategoryId = Guid.NewGuid(),
            OwnerId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            City = "Warsaw",
            Price = 0m,
            Workload = workload,
            WorkLocation = workLocation,
            MinSalary = minSalary,
            MaxSalary = maxSalary,
            ExperienceRequired = experienceRequired,
            PostSettings = new PostSettings
            {
                Id = Guid.NewGuid(),
                IsDeleted = false,
                IsHidden = false,
                postStatusType = PostStatusType.Published
            },
            SubCategory = new SubCategory
            {
                Id = Guid.NewGuid(),
                CategoryId = Guid.NewGuid(),
                TitleEN = "Work",
                TitlePL = "Praca"
            }
        };
    }

    private RentPost CreateRentPost(
        RentObjectType rentObjectType,
        int numberOfRooms,
        decimal area,
        int floor)
    {
        return new RentPost
        {
            Id = Guid.NewGuid(),
            Title = "Rent Post",
            FullDescription = "Description",
            ShortDescription = "Short",
            SubCategoryId = Guid.NewGuid(),
            OwnerId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            City = "Warsaw",
            Price = 2000m,
            RentObjectType = rentObjectType,
            NumberOfRooms = numberOfRooms,
            Area = area,
            Floor = floor,
            PostSettings = new PostSettings
            {
                Id = Guid.NewGuid(),
                IsDeleted = false,
                IsHidden = false,
                postStatusType = PostStatusType.Published
            },
            SubCategory = new SubCategory
            {
                Id = Guid.NewGuid(),
                CategoryId = Guid.NewGuid(),
                TitleEN = "Rent",
                TitlePL = "Wynajem"
            }
        };
    }
}