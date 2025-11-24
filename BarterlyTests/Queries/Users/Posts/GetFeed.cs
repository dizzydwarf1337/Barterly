using Application.Core.MediatR.Requests;
using Application.DTOs.Posts;
using Application.Queries.Users.Posts.GetFeed;
using AutoMapper;
using Domain.Entities.Posts;
using Domain.Entities.Posts.PostTypes;
using Domain.Enums.Posts;
using Domain.Enums.Users;
using Domain.Interfaces.Queries.Post;
using MockQueryable;
using Moq;

namespace BarterlyUnitTests.Queries.Users.Posts;

public class GetFeed
{
    private readonly Mock<IPostQueryRepository> _postQueryRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetFeedQueryHanlder _handler;

    public GetFeed()
    {
        _postQueryRepositoryMock = new Mock<IPostQueryRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetFeedQueryHanlder(
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
    public async Task Handle_WhenPostsExist_ReturnsAllPosts()
    {
        // Arrange
        var posts = new List<Post>
        {
            CreatePost(viewsCount: 100, visitedPostsCount: 5),
            CreatePost(viewsCount: 200, visitedPostsCount: 10),
            CreatePost(viewsCount: 150, visitedPostsCount: 7)
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
                SubCategoryId = post.SubCategoryId.ToString()
            }).ToList());

        var query = new GetFeedQuery
        {
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid()),
            PageNumber = 10
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(3, result.Value.Count);
    }

    [Fact]
    public async Task Handle_WhenNoPostsExist_ReturnsEmptyCollection()
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

        var query = new GetFeedQuery
        {
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid()),
            PageNumber = 10
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Empty(result.Value);
    }
    
    [Fact]
    public async Task Handle_SortsByViewsCountDescending()
    {
        // Arrange
        var post1Id = Guid.NewGuid();
        var post2Id = Guid.NewGuid();
        var post3Id = Guid.NewGuid();

        var posts = new List<Post>
        {
            CreatePost(post1Id, viewsCount: 100, visitedPostsCount: 5),
            CreatePost(post2Id, viewsCount: 300, visitedPostsCount: 5),
            CreatePost(post3Id, viewsCount: 200, visitedPostsCount: 5)
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
                SubCategoryId = post.SubCategoryId.ToString()
            }).ToList());

        var query = new GetFeedQuery
        {
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid()),
            PageNumber = 10
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(post2Id.ToString(), result.Value.ElementAt(0).Id); // 300 views
        Assert.Equal(post3Id.ToString(), result.Value.ElementAt(1).Id); // 200 views
        Assert.Equal(post1Id.ToString(), result.Value.ElementAt(2).Id); // 100 views
    }

    [Fact]
    public async Task Handle_SortsByViewsCount()
    {
        // Arrange
        var post1Id = Guid.NewGuid();
        var post2Id = Guid.NewGuid();
        var post3Id = Guid.NewGuid();
        var post4Id = Guid.NewGuid();

        var posts = new List<Post>
        {
            CreatePost(post1Id, viewsCount: 100, visitedPostsCount: 10),
            CreatePost(post2Id, viewsCount: 200, visitedPostsCount: 10),
            CreatePost(post3Id, viewsCount: 300, visitedPostsCount: 5),
            CreatePost(post4Id, viewsCount: 150, visitedPostsCount: 10)
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
                SubCategoryId = post.SubCategoryId.ToString()
            }).ToList());

        var query = new GetFeedQuery
        {
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid()),
            PageNumber = 10
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(post3Id.ToString(), result.Value.ElementAt(0).Id); // 300 views
        Assert.Equal(post2Id.ToString(), result.Value.ElementAt(1).Id); // 200 views
        Assert.Equal(post4Id.ToString(), result.Value.ElementAt(2).Id); // 150 views
        Assert.Equal(post1Id.ToString(), result.Value.ElementAt(3).Id); // 100 vies
    }

    [Fact]
    public async Task Handle_CallsMapperWithCorrectPosts()
    {
        // Arrange
        var posts = new List<Post>
        {
            CreatePost(viewsCount: 100, visitedPostsCount: 5)
        };

        var mockPosts = posts.AsQueryable().BuildMock();

        _postQueryRepositoryMock
            .Setup(x => x.GetAllPosts())
            .Returns(mockPosts);

        _mapperMock
            .Setup(x => x.Map<List<PostPreviewDto>>(It.IsAny<List<Post>>()))
            .Returns(new List<PostPreviewDto>
            {
                new PostPreviewDto { Id = Guid.NewGuid().ToString(), SubCategoryId = Guid.NewGuid().ToString() }
            });

        var query = new GetFeedQuery
        {
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid()),
            PageNumber = 10
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
        var posts = new List<Post>
        {
            CreatePost(viewsCount: 100, visitedPostsCount: 5)
        };

        var mockPosts = posts.AsQueryable().BuildMock();

        _postQueryRepositoryMock
            .Setup(x => x.GetAllPosts())
            .Returns(mockPosts);

        _mapperMock
            .Setup(x => x.Map<List<PostPreviewDto>>(It.IsAny<List<Post>>()))
            .Returns(new List<PostPreviewDto>
            {
                new PostPreviewDto { Id = Guid.NewGuid().ToString(), SubCategoryId = Guid.NewGuid().ToString() }
            });

        var query = new GetFeedQuery
        {
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid()),
            PageNumber = 10
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Null(result.Error);
    }
    /*
    [Fact]
    public async Task Handle_WithMultiplePosts_ReturnsAllInCorrectOrder()
    {
        // Arrange
        var posts = new List<Post>();
        for (int i = 1; i <= 10; i++)
        {
            posts.Add(CreatePost(viewsCount: i * 100, visitedPostsCount: i));
        }

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

        var query = new GetFeedQuery
        {
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid()),
            PageNumber = 10
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(10, result.Value.Count);
    }
    */

    // Helper method
    private Post CreatePost(int viewsCount, int visitedPostsCount)
    {
        return CreatePost(Guid.NewGuid(), viewsCount, visitedPostsCount);
    }

    private Post CreatePost(Guid postId, int viewsCount, int visitedPostsCount)
    {
        var visitedPosts = new List<VisitedPost>();
        for (int i = 0; i < visitedPostsCount; i++)
        {
            visitedPosts.Add(new VisitedPost
            {
                PostId = postId,
                UserId = Guid.NewGuid(),
                LastVisitedAt = DateTime.UtcNow
            });
        }

        return new CommonPost
        {
            Id = postId,
            Title = "Test Post",
            FullDescription = "Description",
            ShortDescription = "Short",
            SubCategoryId = Guid.NewGuid(),
            OwnerId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            ViewsCount = viewsCount,
            VisitedPosts = visitedPosts,
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