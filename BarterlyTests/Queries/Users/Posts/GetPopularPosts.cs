using Application.Core.MediatR.Requests;
using Application.DTOs.Posts;
using Application.Queries.Users.Posts.GetPopularPosts;
using AutoMapper;
using Domain.Entities.Posts;
using Domain.Entities.Posts.PostTypes;
using Domain.Entities.Users;
using Domain.Enums.Posts;
using Domain.Enums.Users;
using Domain.Interfaces.Queries.Post;
using Domain.Interfaces.Queries.User;
using MockQueryable;
using Moq;

namespace BarterlyUnitTests.Queries.Users.Posts;

public class GetPopularPosts
{
    private readonly Mock<IUserActivityQueryRepository> _userActivityQueryRepositoryMock;
    private readonly Mock<IPostQueryRepository> _postQueryRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetPopularPostsQueryHandler _handler;

    public GetPopularPosts()
    {
        _userActivityQueryRepositoryMock = new Mock<IUserActivityQueryRepository>();
        _postQueryRepositoryMock = new Mock<IPostQueryRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetPopularPostsQueryHandler(
            _userActivityQueryRepositoryMock.Object,
            _postQueryRepositoryMock.Object,
            _mapperMock.Object
        );
    }

    private AuthorizeData CreateAuthorizeData(Guid userId)
    {
        return new AuthorizeData(
            userId,
            new List<UserRoles> { UserRoles.User },
            "test-token",
            null
        );
    }

    [Fact]
    public async Task Handle_WhenUserHasActivityWithCity_ReturnsPostsFromThatCity()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var userActivity = new UserActivitySummary()
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            MostViewedCities = "Warsaw, Krakow" 
        };

        var posts = new List<Post>
        {
            CreatePost(Guid.NewGuid(), "Warsaw", viewsCount: 100),
            CreatePost(Guid.NewGuid(), "Krakow", viewsCount: 200),
            CreatePost(Guid.NewGuid(), "Warsaw", viewsCount: 150)
        };

        var mockPosts = posts.AsQueryable().BuildMock();

        _userActivityQueryRepositoryMock
            .Setup(x => x.GetUserActivityByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(userActivity);

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

        var query = new GetPopularPostsQuery
        {
            AuthorizeData = CreateAuthorizeData(userId),
            Count = 5
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.Count); // Only Warsaw posts
    }

    [Fact]
    public async Task Handle_WhenUserHasActivityWithoutCities_ReturnsAllPosts()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var userActivity = new UserActivitySummary
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            MostViewedCities = ""
        };

        var posts = new List<Post>
        {
            CreatePost(Guid.NewGuid(), "Warsaw", viewsCount: 100),
            CreatePost(Guid.NewGuid(), "Krakow", viewsCount: 200),
            CreatePost(Guid.NewGuid(), "Gdansk", viewsCount: 150)
        };

        var mockPosts = posts.AsQueryable().BuildMock();

        _userActivityQueryRepositoryMock
            .Setup(x => x.GetUserActivityByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(userActivity);

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

        var query = new GetPopularPostsQuery
        {
            AuthorizeData = CreateAuthorizeData(userId),
            Count = 5
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(3, result.Value.Count); // All posts
    }

    [Fact]
    public async Task Handle_UsesFirstCityFromMostViewedCities()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var userActivity = new UserActivitySummary()
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            MostViewedCities = "Warsaw, Krakow, Gdansk"
        };

        var posts = new List<Post>
        {
            CreatePost(Guid.NewGuid(), "Warsaw", viewsCount: 100),
            CreatePost(Guid.NewGuid(), "Krakow", viewsCount: 200),
            CreatePost(Guid.NewGuid(), "Gdansk", viewsCount: 150)
        };

        var mockPosts = posts.AsQueryable().BuildMock();

        _userActivityQueryRepositoryMock
            .Setup(x => x.GetUserActivityByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(userActivity);

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

        var query = new GetPopularPostsQuery
        {
            AuthorizeData = CreateAuthorizeData(userId),
            Count = 5
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Value); // Only Warsaw (first city)
    }

    [Fact]
    public async Task Handle_CityFilteringIsCaseInsensitive()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var userActivity = new UserActivitySummary
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            MostViewedCities = "WaRsAw"
        };

        var posts = new List<Post>
        {
            CreatePost(Guid.NewGuid(), "warsaw", viewsCount: 100),
            CreatePost(Guid.NewGuid(), "WARSAW", viewsCount: 200),
            CreatePost(Guid.NewGuid(), "Warsaw", viewsCount: 150),
            CreatePost(Guid.NewGuid(), "Krakow", viewsCount: 300)
        };

        var mockPosts = posts.AsQueryable().BuildMock();

        _userActivityQueryRepositoryMock
            .Setup(x => x.GetUserActivityByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(userActivity);

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

        var query = new GetPopularPostsQuery
        {
            AuthorizeData = CreateAuthorizeData(userId),
            Count = 5
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(3, result.Value.Count); // All Warsaw variations
    }

    [Fact]
    public async Task Handle_SortsByViewsCountAscending()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var post1Id = Guid.NewGuid();
        var post2Id = Guid.NewGuid();
        var post3Id = Guid.NewGuid();

        var userActivity = new UserActivitySummary()
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            MostViewedCities = ""
        };

        var posts = new List<Post>
        {
            CreatePost(post1Id, "Warsaw", viewsCount: 300),
            CreatePost(post2Id, "Warsaw", viewsCount: 100),
            CreatePost(post3Id, "Warsaw", viewsCount: 200)
        };

        var mockPosts = posts.AsQueryable().BuildMock();

        _userActivityQueryRepositoryMock
            .Setup(x => x.GetUserActivityByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(userActivity);

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

        var query = new GetPopularPostsQuery
        {
            AuthorizeData = CreateAuthorizeData(userId),
            Count = 5
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        // OrderBy сортирует по возрастанию
        Assert.Equal(post2Id.ToString(), result.Value.ElementAt(0).Id); // 100 views
        Assert.Equal(post3Id.ToString(), result.Value.ElementAt(1).Id); // 200 views
        Assert.Equal(post1Id.ToString(), result.Value.ElementAt(2).Id); // 300 views
    }

    [Fact]
    public async Task Handle_WhenNoPostsExist_ReturnsEmptyCollection()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var userActivity = new UserActivitySummary()
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            MostViewedCities = ""
        };

        var emptyPosts = new List<Post>();
        var mockPosts = emptyPosts.AsQueryable().BuildMock();

        _userActivityQueryRepositoryMock
            .Setup(x => x.GetUserActivityByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(userActivity);

        _postQueryRepositoryMock
            .Setup(x => x.GetAllPosts())
            .Returns(mockPosts);

        _mapperMock
            .Setup(x => x.Map<List<PostPreviewDto>>(It.IsAny<List<Post>>()))
            .Returns(new List<PostPreviewDto>());

        var query = new GetPopularPostsQuery
        {
            AuthorizeData = CreateAuthorizeData(userId),
            Count = 5
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Value);
    }

    [Fact]
    public async Task Handle_CallsUserActivityRepositoryWithCorrectUserId()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var userActivity = new UserActivitySummary()
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            MostViewedCities = ""
        };

        var emptyPosts = new List<Post>();
        var mockPosts = emptyPosts.AsQueryable().BuildMock();

        _userActivityQueryRepositoryMock
            .Setup(x => x.GetUserActivityByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(userActivity);

        _postQueryRepositoryMock
            .Setup(x => x.GetAllPosts())
            .Returns(mockPosts);

        _mapperMock
            .Setup(x => x.Map<List<PostPreviewDto>>(It.IsAny<List<Post>>()))
            .Returns(new List<PostPreviewDto>());

        var query = new GetPopularPostsQuery
        {
            AuthorizeData = CreateAuthorizeData(userId),
            Count = 5
        };

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _userActivityQueryRepositoryMock.Verify(
            x => x.GetUserActivityByUserIdAsync(userId, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_CallsMapperWithCorrectPosts()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var userActivity = new UserActivitySummary()
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            MostViewedCities = ""
        };

        var posts = new List<Post>
        {
            CreatePost(Guid.NewGuid(), "Warsaw", viewsCount: 100)
        };

        var mockPosts = posts.AsQueryable().BuildMock();

        _userActivityQueryRepositoryMock
            .Setup(x => x.GetUserActivityByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(userActivity);

        _postQueryRepositoryMock
            .Setup(x => x.GetAllPosts())
            .Returns(mockPosts);

        _mapperMock
            .Setup(x => x.Map<List<PostPreviewDto>>(It.IsAny<List<Post>>()))
            .Returns(new List<PostPreviewDto>
            {
                new PostPreviewDto { Id = Guid.NewGuid().ToString(), SubCategoryId = Guid.NewGuid().ToString() }
            });

        var query = new GetPopularPostsQuery
        {
            AuthorizeData = CreateAuthorizeData(userId),
            Count = 5
        };

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _mapperMock.Verify(
            x => x.Map<List<PostPreviewDto>>(It.IsAny<List<Post>>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_AlwaysReturnsSuccess()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var userActivity = new UserActivitySummary()
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            MostViewedCities = ""
        };

        var posts = new List<Post>
        {
            CreatePost(Guid.NewGuid(), "Warsaw", viewsCount: 100)
        };

        var mockPosts = posts.AsQueryable().BuildMock();

        _userActivityQueryRepositoryMock
            .Setup(x => x.GetUserActivityByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(userActivity);

        _postQueryRepositoryMock
            .Setup(x => x.GetAllPosts())
            .Returns(mockPosts);

        _mapperMock
            .Setup(x => x.Map<List<PostPreviewDto>>(It.IsAny<List<Post>>()))
            .Returns(new List<PostPreviewDto>
            {
                new PostPreviewDto { Id = Guid.NewGuid().ToString(), SubCategoryId = Guid.NewGuid().ToString() }
            });

        var query = new GetPopularPostsQuery
        {
            AuthorizeData = CreateAuthorizeData(userId),
            Count = 5
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Null(result.Error);
    }

    // Helper method
    private Post CreatePost(Guid postId, string city, int viewsCount)
    {
        return new CommonPost
        {
            Id = postId,
            Title = "Test Post",
            FullDescription = "Description",
            ShortDescription = "Short",
            SubCategoryId = Guid.NewGuid(),
            OwnerId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            City = city,
            ViewsCount = viewsCount,
            PostSettings = new PostSettings
            {
                Id = Guid.NewGuid(),
                IsDeleted = false,
                IsHidden = false,
                postStatusType = PostStatusType.Published
            }
        };
    }
}