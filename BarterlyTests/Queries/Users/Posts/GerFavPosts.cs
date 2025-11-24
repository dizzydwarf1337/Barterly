using Application.Core.MediatR.Requests;
using Application.DTOs.Posts;
using Application.Queries.Users.Posts.GetFavPosts;
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

public class GetFavPosts
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IPostQueryRepository> _postQueryRepositoryMock;
    private readonly Mock<IUserFavPostQueryRepository> _favPostQueryRepositoryMock;
    private readonly GetFavPostsQueryHandler _handler;

    public GetFavPosts()
    {
        _mapperMock = new Mock<IMapper>();
        _postQueryRepositoryMock = new Mock<IPostQueryRepository>();
        _favPostQueryRepositoryMock = new Mock<IUserFavPostQueryRepository>();
        _handler = new GetFavPostsQueryHandler(
            _mapperMock.Object,
            _postQueryRepositoryMock.Object,
            _favPostQueryRepositoryMock.Object
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
    public async Task Handle_WhenUserHasFavoritePosts_ReturnsPaginatedResults()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var post1Id = Guid.NewGuid();
        var post2Id = Guid.NewGuid();

        var favPosts = new List<UserFavouritePost>
        {
            new UserFavouritePost { UserId = userId, PostId = post1Id },
            new UserFavouritePost { UserId = userId, PostId = post2Id }
        };

        var posts = new List<Post>
        {
            CreatePost(post1Id, isDeleted: false, isHidden: false),
            CreatePost(post2Id, isDeleted: false, isHidden: false)
        };

        var mockPosts = posts.AsQueryable().BuildMock();

        _favPostQueryRepositoryMock
            .Setup(x => x.GetUserFavPostsByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(favPosts);

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

        var query = new GetFavPostsQuery
        {
            AuthorizeData = CreateAuthorizeData(userId),
            FilterBy = new GetFavPostsQuery.FilterSpecification
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
        Assert.Equal(2, result.Value.Items.Count);
        Assert.Equal(2, result.Value.TotalCount);
        Assert.Equal(1, result.Value.TotalPages);
    }

    [Fact]
    public async Task Handle_WhenUserHasNoFavoritePosts_ReturnsEmptyResult()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var emptyFavPosts = new List<UserFavouritePost>();
        var emptyPosts = new List<Post>();

        var mockPosts = emptyPosts.AsQueryable().BuildMock();

        _favPostQueryRepositoryMock
            .Setup(x => x.GetUserFavPostsByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(emptyFavPosts);

        _postQueryRepositoryMock
            .Setup(x => x.GetAllPosts())
            .Returns(mockPosts);

        _mapperMock
            .Setup(x => x.Map<List<PostPreviewDto>>(It.IsAny<List<Post>>()))
            .Returns(new List<PostPreviewDto>());

        var query = new GetFavPostsQuery
        {
            AuthorizeData = CreateAuthorizeData(userId),
            FilterBy = new GetFavPostsQuery.FilterSpecification
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
    public async Task Handle_ExcludesDeletedPosts()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var post1Id = Guid.NewGuid();
        var post2Id = Guid.NewGuid();
        var post3Id = Guid.NewGuid();

        var favPosts = new List<UserFavouritePost>
        {
            new UserFavouritePost { UserId = userId, PostId = post1Id },
            new UserFavouritePost { UserId = userId, PostId = post2Id },
            new UserFavouritePost { UserId = userId, PostId = post3Id }
        };

        var posts = new List<Post>
        {
            CreatePost(post1Id, isDeleted: false, isHidden: false),
            CreatePost(post2Id, isDeleted: true, isHidden: false),
            CreatePost(post3Id, isDeleted: false, isHidden: false)
        };

        var mockPosts = posts.AsQueryable().BuildMock();

        _favPostQueryRepositoryMock
            .Setup(x => x.GetUserFavPostsByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(favPosts);

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

        var query = new GetFavPostsQuery
        {
            AuthorizeData = CreateAuthorizeData(userId),
            FilterBy = new GetFavPostsQuery.FilterSpecification
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
        Assert.Equal(2, result.Value.Items.Count);
    }

    [Fact]
    public async Task Handle_ExcludesHiddenPosts()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var post1Id = Guid.NewGuid();
        var post2Id = Guid.NewGuid();
        var post3Id = Guid.NewGuid();

        var favPosts = new List<UserFavouritePost>
        {
            new UserFavouritePost { UserId = userId, PostId = post1Id },
            new UserFavouritePost { UserId = userId, PostId = post2Id },
            new UserFavouritePost { UserId = userId, PostId = post3Id }
        };

        var posts = new List<Post>
        {
            CreatePost(post1Id, isDeleted: false, isHidden: false),
            CreatePost(post2Id, isDeleted: false, isHidden: true),
            CreatePost(post3Id, isDeleted: false, isHidden: false)
        };

        var mockPosts = posts.AsQueryable().BuildMock();

        _favPostQueryRepositoryMock
            .Setup(x => x.GetUserFavPostsByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(favPosts);

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

        var query = new GetFavPostsQuery
        {
            AuthorizeData = CreateAuthorizeData(userId),
            FilterBy = new GetFavPostsQuery.FilterSpecification
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
        Assert.Equal(2, result.Value.Items.Count);
    }

    [Fact]
    public async Task Handle_ExcludesDeletedAndHiddenPosts()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var post1Id = Guid.NewGuid();
        var post2Id = Guid.NewGuid();
        var post3Id = Guid.NewGuid();
        var post4Id = Guid.NewGuid();

        var favPosts = new List<UserFavouritePost>
        {
            new UserFavouritePost { UserId = userId, PostId = post1Id },
            new UserFavouritePost { UserId = userId, PostId = post2Id },
            new UserFavouritePost { UserId = userId, PostId = post3Id },
            new UserFavouritePost { UserId = userId, PostId = post4Id }
        };

        var posts = new List<Post>
        {
            CreatePost(post1Id, isDeleted: false, isHidden: false),
            CreatePost(post2Id, isDeleted: true, isHidden: false),
            CreatePost(post3Id, isDeleted: false, isHidden: true),
            CreatePost(post4Id, isDeleted: true, isHidden: true)
        };

        var mockPosts = posts.AsQueryable().BuildMock();

        _favPostQueryRepositoryMock
            .Setup(x => x.GetUserFavPostsByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(favPosts);

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

        var query = new GetFavPostsQuery
        {
            AuthorizeData = CreateAuthorizeData(userId),
            FilterBy = new GetFavPostsQuery.FilterSpecification
            {
                PageSize = 10,
                PageNumber = 1
            }
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Value.TotalCount);
        Assert.Single(result.Value.Items);
    }

    [Fact]
    public async Task Handle_WhenPaginationApplied_ReturnsCorrectPage()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var favPosts = new List<UserFavouritePost>();
        var posts = new List<Post>();

        for (int i = 0; i < 25; i++)
        {
            var postId = Guid.NewGuid();
            favPosts.Add(new UserFavouritePost { UserId = userId, PostId = postId });
            posts.Add(CreatePost(postId, isDeleted: false, isHidden: false));
        }

        var mockPosts = posts.AsQueryable().BuildMock();

        _favPostQueryRepositoryMock
            .Setup(x => x.GetUserFavPostsByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(favPosts);

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

        var query = new GetFavPostsQuery
        {
            AuthorizeData = CreateAuthorizeData(userId),
            FilterBy = new GetFavPostsQuery.FilterSpecification
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
    public async Task Handle_CalculatesTotalPagesCorrectly()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var favPosts = new List<UserFavouritePost>();
        var posts = new List<Post>();

        for (int i = 0; i < 23; i++)
        {
            var postId = Guid.NewGuid();
            favPosts.Add(new UserFavouritePost { UserId = userId, PostId = postId });
            posts.Add(CreatePost(postId, isDeleted: false, isHidden: false));
        }

        var mockPosts = posts.AsQueryable().BuildMock();

        _favPostQueryRepositoryMock
            .Setup(x => x.GetUserFavPostsByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(favPosts);

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

        var query = new GetFavPostsQuery
        {
            AuthorizeData = CreateAuthorizeData(userId),
            FilterBy = new GetFavPostsQuery.FilterSpecification
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
        Assert.Equal(3, result.Value.TotalPages); // Ceiling(23/10) = 3
    }

    [Fact]
    public async Task Handle_OnlyReturnsPostsInFavorites()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var favPost1Id = Guid.NewGuid();
        var favPost2Id = Guid.NewGuid();
        var nonFavPostId = Guid.NewGuid();

        var favPosts = new List<UserFavouritePost>
        {
            new UserFavouritePost { UserId = userId, PostId = favPost1Id },
            new UserFavouritePost { UserId = userId, PostId = favPost2Id }
        };

        var posts = new List<Post>
        {
            CreatePost(favPost1Id, isDeleted: false, isHidden: false),
            CreatePost(favPost2Id, isDeleted: false, isHidden: false),
            CreatePost(nonFavPostId, isDeleted: false, isHidden: false)
        };

        var mockPosts = posts.AsQueryable().BuildMock();

        _favPostQueryRepositoryMock
            .Setup(x => x.GetUserFavPostsByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(favPosts);

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

        var query = new GetFavPostsQuery
        {
            AuthorizeData = CreateAuthorizeData(userId),
            FilterBy = new GetFavPostsQuery.FilterSpecification
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
        Assert.Equal(2, result.Value.Items.Count);
    }

    [Fact]
    public async Task Handle_CallsFavPostRepositoryWithCorrectUserId()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var emptyFavPosts = new List<UserFavouritePost>();
        var emptyPosts = new List<Post>();

        var mockPosts = emptyPosts.AsQueryable().BuildMock();

        _favPostQueryRepositoryMock
            .Setup(x => x.GetUserFavPostsByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(emptyFavPosts);

        _postQueryRepositoryMock
            .Setup(x => x.GetAllPosts())
            .Returns(mockPosts);

        _mapperMock
            .Setup(x => x.Map<List<PostPreviewDto>>(It.IsAny<List<Post>>()))
            .Returns(new List<PostPreviewDto>());

        var query = new GetFavPostsQuery
        {
            AuthorizeData = CreateAuthorizeData(userId),
            FilterBy = new GetFavPostsQuery.FilterSpecification
            {
                PageSize = 10,
                PageNumber = 1
            }
        };

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _favPostQueryRepositoryMock.Verify(
            x => x.GetUserFavPostsByUserIdAsync(userId, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_CallsMapperWithCorrectPosts()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var postId = Guid.NewGuid();

        var favPosts = new List<UserFavouritePost>
        {
            new UserFavouritePost { UserId = userId, PostId = postId }
        };

        var posts = new List<Post>
        {
            CreatePost(postId, isDeleted: false, isHidden: false)
        };

        var mockPosts = posts.AsQueryable().BuildMock();

        _favPostQueryRepositoryMock
            .Setup(x => x.GetUserFavPostsByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(favPosts);

        _postQueryRepositoryMock
            .Setup(x => x.GetAllPosts())
            .Returns(mockPosts);

        _mapperMock
            .Setup(x => x.Map<List<PostPreviewDto>>(It.IsAny<List<Post>>()))
            .Returns(new List<PostPreviewDto>
            {
                new PostPreviewDto { Id = postId.ToString(), SubCategoryId = Guid.NewGuid().ToString() }
            });

        var query = new GetFavPostsQuery
        {
            AuthorizeData = CreateAuthorizeData(userId),
            FilterBy = new GetFavPostsQuery.FilterSpecification
            {
                PageSize = 10,
                PageNumber = 1
            }
        };

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _mapperMock.Verify(
            x => x.Map<List<PostPreviewDto>>(It.IsAny<List<Post>>()),
            Times.Once);
    }

    // Helper method
    private Post CreatePost(Guid postId, bool isDeleted, bool isHidden)
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
            PostSettings = new PostSettings
            {
                Id = Guid.NewGuid(),
                IsDeleted = isDeleted,
                IsHidden = isHidden,
                postStatusType = PostStatusType.Published
            }
        };
    }
}