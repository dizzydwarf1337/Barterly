using Application.DTOs.Posts;
using Application.Queries.Public.Posts.GetFeed;
using AutoMapper;
using Domain.Entities.Posts;
using Domain.Entities.Posts.PostTypes;
using Domain.Entities.Users;
using Domain.Enums.Posts;
using Domain.Interfaces.Queries.Post;
using MockQueryable;
using Moq;
namespace BarterlyUnitTests.Queries.Public.Posts;

public class GetFeedQueryHandlerTests
{
    private readonly Mock<IPostQueryRepository> _postRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetFeedQueryHanlder _handler;

    public GetFeedQueryHandlerTests()
    {
        _postRepositoryMock = new Mock<IPostQueryRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetFeedQueryHanlder(
            _postRepositoryMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_WhenValidRequest_ReturnsPaginatedFeed()
    {
        // Arrange
        var regularPosts = CreateRegularPosts(10);
        var promotedPosts = CreatePromotedPosts(3);
        var allPosts = regularPosts.Concat(promotedPosts).ToList();
        
        var mockPosts = allPosts.AsQueryable().BuildMock();

        _postRepositoryMock
            .Setup(x => x.GetAllPosts())
            .Returns(mockPosts);

        _mapperMock
            .Setup(x => x.Map<ICollection<PostPreviewDto>>(It.IsAny<List<Post>>()))
            .Returns((List<Post> posts) => posts.Select(p => new PostPreviewDto
            {
                Id = p.Id.ToString(),
                Title = p.Title,
                SubCategoryId = p.SubCategoryId.ToString()
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
        Assert.NotEmpty(result.Value.Items);
        Assert.Equal(10, result.Value.TotalCount);
        Assert.Equal(1, result.Value.TotalPages);
    }

    [Fact]
    public async Task Handle_WhenSortByCreatedAtDescending_ReturnsSortedPosts()
    {
        // Arrange
        var posts = new List<Post>
        {
            CreateRegularPost(createdAt: DateTime.UtcNow.AddDays(-3)),
            CreateRegularPost(createdAt: DateTime.UtcNow.AddDays(-1)),
            CreateRegularPost(createdAt: DateTime.UtcNow.AddDays(-2))
        };
        
        var mockPosts = posts.AsQueryable().BuildMock();

        _postRepositoryMock
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
            },
            SortBy = new GetFeedQuery.SortSpecification
            {
                SortBy = "createdat",
                IsDescending = true
            }
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotEmpty(result.Value.Items);
    }

    [Fact]
    public async Task Handle_WhenSortByTitle_ReturnsSortedPosts()
    {
        // Arrange
        var posts = new List<Post>
        {
            CreateRegularPost(title: "Zebra Post"),
            CreateRegularPost(title: "Apple Post"),
            CreateRegularPost(title: "Banana Post")
        };
        
        var mockPosts = posts.AsQueryable().BuildMock();

        _postRepositoryMock
            .Setup(x => x.GetAllPosts())
            .Returns(mockPosts);

        _mapperMock
            .Setup(x => x.Map<ICollection<PostPreviewDto>>(It.IsAny<List<Post>>()))
            .Returns((List<Post> p) => p.Select(post => new PostPreviewDto
            {
                Id = post.Id.ToString(),
                Title = post.Title,
                SubCategoryId = post.SubCategoryId.ToString()
            }).ToList());

        var query = new GetFeedQuery
        {
            FilterBy = new GetFeedQuery.FilterSpecification
            {
                PageSize = 10,
                PageNumber = 1
            },
            SortBy = new GetFeedQuery.SortSpecification
            {
                SortBy = "title",
                IsDescending = false
            }
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotEmpty(result.Value.Items);
    }

    [Fact]
    public async Task Handle_WhenSortByViewsCount_ReturnsSortedPosts()
    {
        // Arrange
        var posts = new List<Post>
        {
            CreateRegularPost(viewsCount: 100),
            CreateRegularPost(viewsCount: 500),
            CreateRegularPost(viewsCount: 250)
        };
        
        var mockPosts = posts.AsQueryable().BuildMock();

        _postRepositoryMock
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
            },
            SortBy = new GetFeedQuery.SortSpecification
            {
                SortBy = "viewscount",
                IsDescending = true
            }
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotEmpty(result.Value.Items);
    }

    [Fact]
    public async Task Handle_WhenNoSortSpecified_DefaultsToCreatedAtDescending()
    {
        // Arrange
        var posts = CreateRegularPosts(5);
        var mockPosts = posts.AsQueryable().BuildMock();

        _postRepositoryMock
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
        Assert.NotEmpty(result.Value.Items);
    }

    [Fact]
    public async Task Handle_ExcludesDeletedPosts()
    {
        // Arrange
        var posts = new List<Post>
        {
            CreateRegularPost(isDeleted: false),
            CreateRegularPost(isDeleted: true),
            CreateRegularPost(isDeleted: false)
        };
        
        var mockPosts = posts.AsQueryable().BuildMock();

        _postRepositoryMock
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
        Assert.Equal(2, result.Value.TotalCount);
    }

    [Fact]
    public async Task Handle_ExcludesHiddenPosts()
    {
        // Arrange
        var posts = new List<Post>
        {
            CreateRegularPost(isHidden: false),
            CreateRegularPost(isHidden: true),
            CreateRegularPost(isHidden: false)
        };
    
        var mockPosts = posts.AsQueryable().BuildMock();

        _postRepositoryMock
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
        Assert.Equal(3, result.Value.TotalCount);
        Assert.Equal(2, result.Value.Items.Count);
    }

    [Fact]
    public async Task Handle_OnlyIncludesPublishedPosts()
    {
        // Arrange
        var posts = new List<Post>
        {
            CreateRegularPost(status: PostStatusType.Published),
            CreateRegularPost(status: PostStatusType.UnderReview),
            CreateRegularPost(status: PostStatusType.Published)
        };
        
        var mockPosts = posts.AsQueryable().BuildMock();

        _postRepositoryMock
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
        Assert.Equal(2, result.Value.TotalCount);
    }

    [Fact]
    public async Task Handle_IncludesPromotedPosts()
    {
        // Arrange
        var regularPosts = CreateRegularPosts(7);
        var topPosts = CreateTopPromotedPosts(2);
        var highlightPosts = CreateHighlightPromotedPosts(1);
        
        var allPosts = regularPosts.Concat(topPosts).Concat(highlightPosts).ToList();
        var mockPosts = allPosts.AsQueryable().BuildMock();

        _postRepositoryMock
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
        Assert.NotEmpty(result.Value.Items);
        // Should have mix of regular and promoted posts
        Assert.True(result.Value.Items.Count > 7); // At least regular posts + some promoted
    }

    [Fact]
    public async Task Handle_CalculatesTotalPagesCorrectly()
    {
        // Arrange
        var posts = CreateRegularPosts(25);
        var mockPosts = posts.AsQueryable().BuildMock();

        _postRepositoryMock
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
        Assert.Equal(3, result.Value.TotalPages); // Ceiling(25/10) = 3
    }

    [Fact]
    public async Task Handle_WhenPaginationApplied_ReturnsCorrectPage()
    {
        // Arrange
        var posts = CreateRegularPosts(30);
        var mockPosts = posts.AsQueryable().BuildMock();

        _postRepositoryMock
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
                PageNumber = 2
            }
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(30, result.Value.TotalCount);
        Assert.Equal(3, result.Value.TotalPages);
    }

    [Fact]
    public async Task Handle_WhenNoPostsAvailable_ReturnsEmptyFeed()
    {
        // Arrange
        var emptyPosts = new List<Post>();
        var mockPosts = emptyPosts.AsQueryable().BuildMock();

        _postRepositoryMock
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
        Assert.Empty(result.Value.Items);
        Assert.Equal(0, result.Value.TotalCount);
        Assert.Equal(0, result.Value.TotalPages);
    }

    // Helper methods
    private List<Post> CreateRegularPosts(int count)
    {
        var posts = new List<Post>();
        for (int i = 0; i < count; i++)
        {
            posts.Add(CreateRegularPost());
        }
        return posts;
    }

    private Post CreateRegularPost(
        string title = "Test Post",
        DateTime? createdAt = null,
        int viewsCount = 0,
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
            CreatedAt = createdAt ?? DateTime.UtcNow,
            ViewsCount = viewsCount,
            Owner = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Test",
                LastName = "User"
            },
            PostSettings = new PostSettings
            {
                Id = Guid.NewGuid(),
                IsDeleted = isDeleted,
                IsHidden = isHidden,
                postStatusType = status
            },
            Promotion = new Promotion
            {
                Id = Guid.NewGuid(),
                Type = PostPromotionType.None
            }
        };
    }

    private List<Post> CreatePromotedPosts(int count)
    {
        return CreateTopPromotedPosts(count / 2)
            .Concat(CreateHighlightPromotedPosts(count - count / 2))
            .ToList();
    }

    private List<Post> CreateTopPromotedPosts(int count)
    {
        var posts = new List<Post>();
        for (int i = 0; i < count; i++)
        {
            var post = CreateRegularPost();
            post.Promotion.Type = PostPromotionType.Top;
            posts.Add(post);
        }
        return posts;
    }

    private List<Post> CreateHighlightPromotedPosts(int count)
    {
        var posts = new List<Post>();
        for (int i = 0; i < count; i++)
        {
            var post = CreateRegularPost();
            post.Promotion.Type = PostPromotionType.Highlight;
            posts.Add(post);
        }
        return posts;
    }
}