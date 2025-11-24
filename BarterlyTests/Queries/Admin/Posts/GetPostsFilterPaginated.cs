using Application.DTOs.Posts;
using Application.Queries.Admins.Posts.GetPostFiltredPaginated;
using AutoMapper;
using Domain.Entities.Categories;
using Domain.Entities.Posts;
using Domain.Entities.Posts.PostTypes;
using Domain.Interfaces.Queries.Post;
using MockQueryable;
using Moq;

namespace BarterlyUnitTests.Queries.Admin.Posts;

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
    public async Task Handle_WhenFilterByIsNull_ReturnsFailure()
    {
        // Arrange
        var query = new GetPostsQuery
        {
            FilterBy = null
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Invalid filter parameters.", result.Error);
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(-1, 1)]
    [InlineData(10, 0)]
    [InlineData(10, -1)]
    public async Task Handle_WhenPaginationParametersInvalid_ReturnsFailure(int pageSize, int pageNumber)
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
    public async Task Handle_WhenValidRequest_ReturnsPaginatedResults()
    {
        // Arrange
        var posts = CreateTestPosts(15);
        var mockPosts = posts.AsQueryable().BuildMock();

        var postDtos = new List<PostPreviewDto>
        {
            new PostPreviewDto { Id = "1", Title = "Post 1", SubCategoryId = Guid.NewGuid().ToString() },
            new PostPreviewDto { Id = "2", Title = "Post 2", SubCategoryId = Guid.NewGuid().ToString() }
        };

        _postQueryRepositoryMock
            .Setup(x => x.GetAllPosts())
            .Returns(mockPosts);

        _mapperMock
            .Setup(x => x.Map<List<PostPreviewDto>>(It.IsAny<List<Post>>()))
            .Returns(postDtos);

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
        Assert.Equal(postDtos, result.Value.Items);
    }

    [Fact]
    public async Task Handle_WhenSearchFilterApplied_ReturnsMatchingPosts()
    {
        // Arrange
        var posts = new List<Post>
        {
            CreateTestPost("Test Post 1"),
            CreateTestPost("Another Post"),
            CreateTestPost("Test Post 2")
        };
        var mockPosts = posts.AsQueryable().BuildMock();

        _postQueryRepositoryMock
            .Setup(x => x.GetAllPosts())
            .Returns(mockPosts);

        _mapperMock
            .Setup(x => x.Map<List<PostPreviewDto>>(It.IsAny<List<Post>>()))
            .Returns((List<Post> source) => source.Select(p => new PostPreviewDto 
            { 
                Id = p.Id.ToString(), 
                Title = p.Title,
                SubCategoryId = p.SubCategoryId.ToString()
            }).ToList());

        var query = new GetPostsQuery
        {
            FilterBy = new GetPostsQuery.FilterSpecification
            {
                Search = "test",
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
            CreateTestPostWithCategory(categoryId),
            CreateTestPostWithCategory(Guid.NewGuid()),
            CreateTestPostWithCategory(categoryId)
        };
        var mockPosts = posts.AsQueryable().BuildMock();

        _postQueryRepositoryMock
            .Setup(x => x.GetAllPosts())
            .Returns(mockPosts);

        _mapperMock
            .Setup(x => x.Map<List<PostPreviewDto>>(It.IsAny<List<Post>>()))
            .Returns(new List<PostPreviewDto> 
            { 
                new PostPreviewDto 
                { 
                    Id = Guid.NewGuid().ToString(),
                    SubCategoryId = Guid.NewGuid().ToString() 
                } 
            });

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
            CreateTestPostWithSubCategory(subCategoryId),
            CreateTestPostWithSubCategory(Guid.NewGuid()),
            CreateTestPostWithSubCategory(subCategoryId)
        };
        var mockPosts = posts.AsQueryable().BuildMock();

        _postQueryRepositoryMock
            .Setup(x => x.GetAllPosts())
            .Returns(mockPosts);

        _mapperMock
            .Setup(x => x.Map<List<PostPreviewDto>>(It.IsAny<List<Post>>()))
            .Returns(new List<PostPreviewDto> 
            { 
                new PostPreviewDto 
                { 
                    Id = Guid.NewGuid().ToString(),
                    SubCategoryId = subCategoryId.ToString() 
                } 
            });

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
            CreateTestPostWithOwner(userId),
            CreateTestPostWithOwner(Guid.NewGuid()),
            CreateTestPostWithOwner(userId)
        };
        var mockPosts = posts.AsQueryable().BuildMock();

        _postQueryRepositoryMock
            .Setup(x => x.GetAllPosts())
            .Returns(mockPosts);

        _mapperMock
            .Setup(x => x.Map<List<PostPreviewDto>>(It.IsAny<List<Post>>()))
            .Returns(new List<PostPreviewDto> 
            { 
                new PostPreviewDto 
                { 
                    Id = Guid.NewGuid().ToString(),
                    SubCategoryId = Guid.NewGuid().ToString() 
                } 
            });

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

    [Fact]
    public async Task Handle_WhenIsActiveFilterApplied_ReturnsActivePosts()
    {
        // Arrange
        var posts = new List<Post>
        {
            CreateTestPostWithSettings(isHidden: false, isDeleted: false),
            CreateTestPostWithSettings(isHidden: true, isDeleted: false),
            CreateTestPostWithSettings(isHidden: false, isDeleted: true),
            CreateTestPostWithSettings(isHidden: false, isDeleted: false)
        };
        var mockPosts = posts.AsQueryable().BuildMock();

        _postQueryRepositoryMock
            .Setup(x => x.GetAllPosts())
            .Returns(mockPosts);

        _mapperMock
            .Setup(x => x.Map<List<PostPreviewDto>>(It.IsAny<List<Post>>()))
            .Returns(new List<PostPreviewDto> 
            { 
                new PostPreviewDto 
                { 
                    Id = Guid.NewGuid().ToString(),
                    SubCategoryId = Guid.NewGuid().ToString() 
                } 
            });

        var query = new GetPostsQuery
        {
            FilterBy = new GetPostsQuery.FilterSpecification
            {
                IsActive = true,
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
    public async Task Handle_WhenIsDeletedFilterApplied_ReturnsDeletedPosts()
    {
        // Arrange
        var posts = new List<Post>
        {
            CreateTestPostWithSettings(isHidden: false, isDeleted: true),
            CreateTestPostWithSettings(isHidden: false, isDeleted: false),
            CreateTestPostWithSettings(isHidden: false, isDeleted: true)
        };
        var mockPosts = posts.AsQueryable().BuildMock();

        _postQueryRepositoryMock
            .Setup(x => x.GetAllPosts())
            .Returns(mockPosts);

        _mapperMock
            .Setup(x => x.Map<List<PostPreviewDto>>(It.IsAny<List<Post>>()))
            .Returns(new List<PostPreviewDto> 
            { 
                new PostPreviewDto 
                { 
                    Id = Guid.NewGuid().ToString(),
                    SubCategoryId = Guid.NewGuid().ToString() 
                } 
            });

        var query = new GetPostsQuery
        {
            FilterBy = new GetPostsQuery.FilterSpecification
            {
                IsDeleted = true,
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
        var posts = new List<Post>
        {
            CreateTestPostWithDate("Post A", DateTime.UtcNow.AddDays(-2)),
            CreateTestPostWithDate("Post C", DateTime.UtcNow.AddDays(-1)),
            CreateTestPostWithDate("Post B", DateTime.UtcNow)
        };
        var mockPosts = posts.AsQueryable().BuildMock();

        _postQueryRepositoryMock
            .Setup(x => x.GetAllPosts())
            .Returns(mockPosts);

        _mapperMock
            .Setup(x => x.Map<List<PostPreviewDto>>(It.IsAny<List<Post>>()))
            .Returns((List<Post> source) => source.Select(p => new PostPreviewDto
            {
                Id = p.Id.ToString(),
                Title = p.Title,
                SubCategoryId = p.SubCategoryId.ToString()
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
        Assert.Equal(3, result.Value.Items.Count);
    }

    [Fact]
    public async Task Handle_WhenPaginationApplied_ReturnsCorrectPage()
    {
        // Arrange
        var posts = CreateTestPosts(25);
        var mockPosts = posts.AsQueryable().BuildMock();

        _postQueryRepositoryMock
            .Setup(x => x.GetAllPosts())
            .Returns(mockPosts);

        _mapperMock
            .Setup(x => x.Map<List<PostPreviewDto>>(It.IsAny<List<Post>>()))
            .Returns((List<Post> source) => source.Select(p => new PostPreviewDto
            {
                Id = p.Id.ToString(),
                SubCategoryId = p.SubCategoryId.ToString()
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
        Assert.Equal(25, result.Value.TotalCount);
        Assert.Equal(3, result.Value.TotalPages);
        Assert.Equal(10, result.Value.Items.Count);
    }

    [Fact]
    public async Task Handle_WhenNoPostsMatch_ReturnsEmptyResult()
    {
        // Arrange
        var posts = new List<Post>();
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
        Assert.Equal(0, result.Value.TotalCount);
        Assert.Equal(0, result.Value.TotalPages);
        Assert.Empty(result.Value.Items);
    }

    // Helper methods
    private List<Post> CreateTestPosts(int count)
    {
        var posts = new List<Post>();
        for (int i = 0; i < count; i++)
        {
            posts.Add(CreateTestPost($"Post {i + 1}"));
        }
        return posts;
    }

    private Post CreateTestPost(string title)
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
            PostSettings = new PostSettings
            {
                Id = Guid.NewGuid(),
                IsHidden = false,
                IsDeleted = false
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

    private Post CreateTestPostWithCategory(Guid categoryId)
    {
        var post = CreateTestPost("Test Post");
        post.SubCategory.CategoryId = categoryId;
        return post;
    }

    private Post CreateTestPostWithSubCategory(Guid subCategoryId)
    {
        var post = CreateTestPost("Test Post");
        post.SubCategoryId = subCategoryId;
        return post;
    }

    private Post CreateTestPostWithOwner(Guid ownerId)
    {
        var post = CreateTestPost("Test Post");
        post.OwnerId = ownerId;
        return post;
    }

    private Post CreateTestPostWithSettings(bool isHidden, bool isDeleted)
    {
        var post = CreateTestPost("Test Post");
        post.PostSettings.IsHidden = isHidden;
        post.PostSettings.IsDeleted = isDeleted;
        return post;
    }

    private Post CreateTestPostWithDate(string title, DateTime createdAt)
    {
        var post = CreateTestPost(title);
        post.CreatedAt = createdAt;
        return post;
    }
}