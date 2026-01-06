using Application.Core.MediatR.Requests;
using Application.DTOs.Posts;
using Application.Queries.Public.Posts.GetFeed;
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
        _handler = new GetFeedQueryHanlder(_postQueryRepositoryMock.Object, _mapperMock.Object);
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
            CreatePost(viewsCount: 100),
            CreatePost(viewsCount: 200),
            CreatePost(viewsCount: 150)
        };

        var mockPosts = posts.AsQueryable().BuildMock();

        _postQueryRepositoryMock
            .Setup(x => x.GetAllPosts())
            .Returns(mockPosts);

        _mapperMock
            .Setup(x => x.Map<ICollection<PostPreviewDto>>(It.IsAny<List<Post>>()))
            .Returns((List<Post> p) => p.Select(post => new PostPreviewDto
            {
                Id = post.Id.ToString(),
                SubCategoryId = post.SubCategoryId.ToString()
            }).ToList());

        var query = new GetFeedQuery
        {
            FilterBy = new GetFeedQuery.FilterSpecification
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
        Assert.Equal(3, result.Value.Items.Count);
        Assert.Equal(3, result.Value.TotalCount);
        Assert.Equal(1, result.Value.TotalPages);
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
            .Setup(x => x.Map<ICollection<PostPreviewDto>>(It.IsAny<List<Post>>()))
            .Returns(new List<PostPreviewDto>());

        var query = new GetFeedQuery
        {
            FilterBy = new GetFeedQuery.FilterSpecification
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
        Assert.Empty(result.Value.Items);
        Assert.Equal(0, result.Value.TotalCount);
    }

    [Fact]
    public async Task Handle_CallsMapperWithCorrectPosts()
    {
        // Arrange
        var posts = new List<Post>
        {
            CreatePost(viewsCount: 100)
        };

        var mockPosts = posts.AsQueryable().BuildMock();

        _postQueryRepositoryMock
            .Setup(x => x.GetAllPosts())
            .Returns(mockPosts);

        _mapperMock
            .Setup(x => x.Map<ICollection<PostPreviewDto>>(It.IsAny<List<Post>>()))
            .Returns(new List<PostPreviewDto>
            {
                new PostPreviewDto { Id = Guid.NewGuid().ToString(), SubCategoryId = Guid.NewGuid().ToString() }
            });

        var query = new GetFeedQuery
        {
            FilterBy = new GetFeedQuery.FilterSpecification
            {
                PageSize = 10,
                PageNumber = 1
            }
        };

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _mapperMock.Verify(
            x => x.Map<ICollection<PostPreviewDto>>(It.IsAny<List<Post>>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_AlwaysReturnsSuccess()
    {
        // Arrange
        var posts = new List<Post>
        {
            CreatePost(viewsCount: 100)
        };

        var mockPosts = posts.AsQueryable().BuildMock();

        _postQueryRepositoryMock
            .Setup(x => x.GetAllPosts())
            .Returns(mockPosts);

        _mapperMock
            .Setup(x => x.Map<ICollection<PostPreviewDto>>(It.IsAny<List<Post>>()))
            .Returns(new List<PostPreviewDto>
            {
                new PostPreviewDto { Id = Guid.NewGuid().ToString(), SubCategoryId = Guid.NewGuid().ToString() }
            });

        var query = new GetFeedQuery
        {
            FilterBy = new GetFeedQuery.FilterSpecification
            {
                PageSize = 10,
                PageNumber = 1
            }
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Null(result.Error);
    }
    
    [Fact]
    public async Task Handle_ExcludesDeletedPosts()
    {
        // Arrange
        var activePost = CreatePost(Guid.NewGuid(), viewsCount: 100);
        var deletedPost = CreatePost(Guid.NewGuid(), viewsCount: 200, isDeleted: true);

        var posts = new List<Post> { activePost, deletedPost };
        var mockPosts = posts.AsQueryable().BuildMock();

        _postQueryRepositoryMock
            .Setup(x => x.GetAllPosts())
            .Returns(mockPosts);

        _mapperMock
            .Setup(x => x.Map<ICollection<PostPreviewDto>>(It.IsAny<List<Post>>()))
            .Returns((List<Post> p) => p.Select(post => new PostPreviewDto
            {
                Id = post.Id.ToString(),
                SubCategoryId = post.SubCategoryId.ToString()
            }).ToList());

        var query = new GetFeedQuery
        {
           FilterBy = new GetFeedQuery.FilterSpecification
            {
                PageSize = 10,
                PageNumber = 1
            }
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Value.Items);
        Assert.Equal(activePost.Id.ToString(), result.Value.Items.First().Id);
    }
    
    [Fact]
    public async Task Handle_ExcludesHiddenPosts()
    {
        // Arrange
        var visiblePost = CreatePost(Guid.NewGuid(), viewsCount: 100);
        var hiddenPost = CreatePost(Guid.NewGuid(), viewsCount: 200, isHidden: true);

        var posts = new List<Post> { visiblePost, hiddenPost };
        var mockPosts = posts.AsQueryable().BuildMock();

        _postQueryRepositoryMock
            .Setup(x => x.GetAllPosts())
            .Returns(mockPosts);

        _mapperMock
            .Setup(x => x.Map<ICollection<PostPreviewDto>>(It.IsAny<List<Post>>()))
            .Returns((List<Post> p) => p.Select(post => new PostPreviewDto
            {
                Id = post.Id.ToString(),
                SubCategoryId = post.SubCategoryId.ToString()
            }).ToList());

        var query = new GetFeedQuery
        {
            FilterBy = new GetFeedQuery.FilterSpecification
            {
                PageSize = 10,
                PageNumber = 1
            }
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Value.Items);
        Assert.Equal(visiblePost.Id.ToString(), result.Value.Items.First().Id);
    }
    
    [Fact]
    public async Task Handle_CalculatesTotalPagesCorrectly()
    {
        // Arrange
        var posts = new List<Post>();
        for (int i = 0; i < 25; i++)
        {
            posts.Add(CreatePost(viewsCount: 100));
        }

        var mockPosts = posts.AsQueryable().BuildMock();

        _postQueryRepositoryMock
            .Setup(x => x.GetAllPosts())
            .Returns(mockPosts);

        _mapperMock
            .Setup(x => x.Map<ICollection<PostPreviewDto>>(It.IsAny<List<Post>>()))
            .Returns((List<Post> p) => p.Select(post => new PostPreviewDto
            {
                Id = post.Id.ToString(),
                SubCategoryId = post.SubCategoryId.ToString()
            }).ToList());

        var query = new GetFeedQuery
        {
            FilterBy = new GetFeedQuery.FilterSpecification
            {
                PageSize = 10,
                PageNumber = 1
            }
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(25, result.Value.TotalCount);
        Assert.Equal(3, result.Value.TotalPages); // 25 posts / 10 per page = 3 pages
    }
    
    // Helper methods
    private Post CreatePost(int viewsCount, bool isDeleted = false, bool isHidden = false)
    {
        return CreatePost(Guid.NewGuid(), viewsCount, isDeleted, isHidden);
    }

    private Post CreatePost(Guid postId, int viewsCount, bool isDeleted = false, bool isHidden = false)
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
            ViewsCount = viewsCount,
            VisitedPosts = new List<VisitedPost>(),
            PostSettings = new PostSettings
            {
                Id = Guid.NewGuid(),
                IsDeleted = isDeleted,
                IsHidden = isHidden,
                postStatusType = PostStatusType.Published
            },
            Promotion = new Promotion()
            {
                Id = Guid.NewGuid(),
                Type = PostPromotionType.None
            }
        };
    }
}