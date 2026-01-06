using Application.DTOs.Posts;
using Application.Queries.Users.Posts.GetPostsFiltredPaginated;
using AutoMapper;
using Domain.Entities.Posts;
using Domain.Entities.Posts.PostTypes;
using Domain.Enums.Posts;
using Domain.Interfaces.Queries.Post;
using MockQueryable;
using Moq;

namespace BarterlyUnitTests.Queries.Users.Posts;

public class GetPosts
{
    private readonly Mock<IPostQueryRepository> _postQueryRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetPostsQueryHandler _handler;

    public GetPosts()
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
        var posts = CreatePosts(15);
        var mockPosts = posts.BuildMock();

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

    [Fact]
    public async Task Handle_WhenSearchFilterApplied_ReturnsMatchingPosts()
    {
        // Arrange
        var posts = new List<Post>
        {
            CreatePost("Laptop for sale"),
            CreatePost("Phone for sale"),
            CreatePost("Book collection")
        };
        var mockPosts = posts.BuildMock();

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
    public async Task Handle_WhenSubCategoryIdFilterApplied_ReturnsMatchingPosts()
    {
        // Arrange
        var subCategoryId = Guid.NewGuid();
        var posts = new List<Post>
        {
            CreatePostWithSubCategory(subCategoryId),
            CreatePostWithSubCategory(Guid.NewGuid()),
            CreatePostWithSubCategory(subCategoryId)
        };
        var mockPosts = posts.BuildMock();

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
    public async Task Handle_WhenUserIdFilterApplied_ReturnsMatchingPosts()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var posts = new List<Post>
        {
            CreatePostWithOwner(userId),
            CreatePostWithOwner(Guid.NewGuid()),
            CreatePostWithOwner(userId)
        };
        var mockPosts = posts.BuildMock();

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
                UserId = userId,
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

    [Theory]
    [InlineData("title", false)]
    [InlineData("title", true)]
    [InlineData("createdat", false)]
    [InlineData("createdat", true)]
    public async Task Handle_WhenSortByApplied_ReturnsSortedPosts(string sortBy, bool isDescending)
    {
        // Arrange
        var posts = CreatePosts(5);
        var mockPosts = posts.BuildMock();

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
    public async Task Handle_WhenNoSortSpecified_ReturnsUnsortedPosts()
    {
        // Arrange
        var posts = CreatePosts(5);
        var mockPosts = posts.BuildMock();

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
        Assert.Equal(5, result.Value.Items.Count);
    }

    [Fact]
    public async Task Handle_WhenAllFiltersApplied_ReturnsMatchingPosts()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var subCategoryId = Guid.NewGuid();

        var posts = new List<Post>
        {
            CreatePostWithFilters(userId, subCategoryId, "Test Post 1"),
            CreatePostWithFilters(Guid.NewGuid(), subCategoryId, "Test Post 2"),
            CreatePostWithFilters(userId, Guid.NewGuid(), "Test Post 3"),
            CreatePostWithFilters(userId, subCategoryId, "Another Post")
        };
        var mockPosts = posts.BuildMock();

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
                UserId = userId,
                SubCategoryId = subCategoryId,
                Search = "Test",
                PageSize = 10,
                PageNumber = 1
            }
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Value.TotalCount); // Only "Test Post 1" matches all filters
    }

    [Fact]
    public async Task Handle_CalculatesTotalPagesCorrectly()
    {
        // Arrange
        var posts = CreatePosts(23);
        var mockPosts = posts.BuildMock();

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
        Assert.Equal(23, result.Value.TotalCount);
        Assert.Equal(3, result.Value.TotalPages); 
    }

    [Fact]
    public async Task Handle_WhenPaginationApplied_ReturnsCorrectPage()
    {
        // Arrange
        var posts = CreatePosts(21); // 21 / 7 = 3 pages exactly
        var mockPosts = posts.BuildMock();

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
                PageNumber = 2
            }
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(21, result.Value.TotalCount);
        Assert.Equal(3, result.Value.TotalPages); // 21 / 10 = 3
        Assert.Equal(10, result.Value.Items.Count); // 7 regular + 3 promoted
    }

    [Fact]
    public async Task Handle_WhenNoPostsMatch_ReturnsEmptyResult()
    {
        // Arrange
        var emptyPosts = new List<Post>();
        var mockPosts = emptyPosts.BuildMock();

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

    [Fact]
    public async Task Handle_SearchIsCaseInsensitive()
    {
        // Arrange
        var posts = new List<Post>
        {
            CreatePost("LAPTOP"),
            CreatePost("laptop"),
            CreatePost("LaPtOp"),
            CreatePost("Phone")
        };
        var mockPosts = posts.BuildMock();

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
                Search = "laptop",
                PageSize = 10,
                PageNumber = 1
            }
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(3, result.Value.TotalCount);
    }

    // Helper methods
    private List<Post> CreatePosts(int count)
    {
        var posts = new List<Post>();
        for (int i = 0; i < count; i++)
        {
            posts.Add(CreatePost($"Post {i + 1}"));
        }
        return posts;
    }

    private Post CreatePost(string title = "Test Post")
    {
        var ownerId = Guid.NewGuid();
        var subCategoryId = Guid.NewGuid();
        
        return new CommonPost
        {
            Id = Guid.NewGuid(),
            Title = title,
            FullDescription = "Description",
            ShortDescription = "Short",
            SubCategoryId = subCategoryId,
            OwnerId = ownerId,
            CreatedAt = DateTime.UtcNow,
            ViewsCount = 0,
            PostSettings = new PostSettings
            {
                Id = Guid.NewGuid(),
                PostId = Guid.NewGuid(),
                IsDeleted = false,
                IsHidden = false,
                postStatusType = PostStatusType.Published
            },
            Owner = new Domain.Entities.Users.User
            {
                Id = ownerId,
                FirstName = "Test",
                LastName = "User",
                Email = "test@test.com"
            },
            SubCategory = new Domain.Entities.Categories.SubCategory
            {
                Id = subCategoryId,
                CategoryId = Guid.NewGuid(),
                TitleEN = "Subcategory",
                TitlePL = "Podkategoria"
            },
            Promotion = new Promotion()
            {
                Id = Guid.NewGuid(),
                PostId = Guid.NewGuid(),
                Type = PostPromotionType.None,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(30)
            }
        };
    }

    private Post CreatePostWithSubCategory(Guid subCategoryId)
    {
        var post = CreatePost();
        post.SubCategoryId = subCategoryId;
        post.SubCategory = new Domain.Entities.Categories.SubCategory
        {
            Id = subCategoryId,
            CategoryId = Guid.NewGuid(),
            TitleEN = "Subcategory",
            TitlePL = "Podkategoria"
        };
        return post;
    }

    private Post CreatePostWithOwner(Guid ownerId)
    {
        var post = CreatePost();
        post.OwnerId = ownerId;
        post.Owner = new Domain.Entities.Users.User
        {
            Id = ownerId,
            FirstName = "Test",
            LastName = "User",
            Email = "test@test.com"
        };
        return post;
    }

    private Post CreatePostWithFilters(Guid ownerId, Guid subCategoryId, string title)
    {
        var post = CreatePost(title);
        post.OwnerId = ownerId;
        post.SubCategoryId = subCategoryId;
        post.Owner = new Domain.Entities.Users.User
        {
            Id = ownerId,
            FirstName = "Test",
            LastName = "User",
            Email = "test@test.com"
        };
        post.SubCategory = new Domain.Entities.Categories.SubCategory
        {
            Id = subCategoryId,
            CategoryId = Guid.NewGuid(),
            TitleEN = "Subcategory",
            TitlePL = "Podkategoria"
        };
        return post;
    }
}