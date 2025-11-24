using Application.DTOs.Posts;
using Application.Queries.Public.Posts.GetPostById;
using AutoMapper;
using Domain.Entities.Posts;
using Domain.Entities.Posts.PostTypes;
using Domain.Entities.Users;
using Domain.Enums.Posts;
using Domain.Interfaces.Queries.Post;
using MockQueryable;
using Moq;
using MockQueryable.Moq;

namespace BarterlyUnitTests.Queries.Users.Posts;

public class GetPostByIdQueryHandlerTests
{
    private readonly Mock<IPostQueryRepository> _postQueryRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetPostByIdQueryHandler _handler;

    public GetPostByIdQueryHandlerTests()
    {
        _postQueryRepositoryMock = new Mock<IPostQueryRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetPostByIdQueryHandler(
            _postQueryRepositoryMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_WhenPostExistsAndNotDeletedOrHidden_ReturnsSuccess()
    {
        // Arrange
        var postId = Guid.NewGuid();
        var ownerId = Guid.NewGuid();
        var subCategoryId = Guid.NewGuid();

        var posts = new List<Post>
        {
            new CommonPost
            {
                Id = postId,
                Title = "Test Post",
                FullDescription = "Full description",
                ShortDescription = "Short description",
                SubCategoryId = subCategoryId,
                OwnerId = ownerId,
                CreatedAt = DateTime.UtcNow,
                Owner = new User
                {
                    Id = ownerId,
                    FirstName = "John",
                    LastName = "Doe"
                },
                PostSettings = new PostSettings
                {
                    Id = Guid.NewGuid(),
                    IsDeleted = false,
                    IsHidden = false,
                    postStatusType = PostStatusType.Published
                }
            }
        };

        var mockPosts = posts.AsQueryable().BuildMock();

        var postDto = new PostDto
        {
            Id = postId.ToString(),
            Title = "Test Post",
            OwnerId = ownerId.ToString(),
            SubCategoryId = subCategoryId.ToString(),
            FullDescription = "Full description",
            ShortDescription = "Short description"
        };

        _postQueryRepositoryMock
            .Setup(x => x.GetAllPosts())
            .Returns(mockPosts);

        _mapperMock
            .Setup(x => x.Map<PostDto>(It.IsAny<Post>()))
            .Returns(postDto);

        var query = new GetPostByIdQuery { PostId = postId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(postDto, result.Value);
        Assert.Null(result.Error);

        _mapperMock.Verify(x => x.Map<PostDto>(It.IsAny<Post>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenPostIsDeleted_ReturnsFailure()
    {
        // Arrange
        var postId = Guid.NewGuid();

        var posts = new List<Post>
        {
            new CommonPost
            {
                Id = postId,
                Title = "Deleted Post",
                FullDescription = "Description",
                ShortDescription = "Short",
                SubCategoryId = Guid.NewGuid(),
                OwnerId = Guid.NewGuid(),
                PostSettings = new PostSettings
                {
                    Id = Guid.NewGuid(),
                    IsDeleted = true,
                    IsHidden = false,
                    postStatusType = PostStatusType.Published
                }
            }
        };

        var mockPosts = posts.AsQueryable().BuildMock();

        _postQueryRepositoryMock
            .Setup(x => x.GetAllPosts())
            .Returns(mockPosts);

        var query = new GetPostByIdQuery { PostId = postId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Null(result.Value);
        Assert.Equal("Post not found or is hidden/deleted.", result.Error);
        Assert.Equal(404, result.StatusCode);

        _mapperMock.Verify(x => x.Map<PostDto>(It.IsAny<Post>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenPostIsHidden_ReturnsFailure()
    {
        // Arrange
        var postId = Guid.NewGuid();

        var posts = new List<Post>
        {
            new CommonPost
            {
                Id = postId,
                Title = "Hidden Post",
                FullDescription = "Description",
                ShortDescription = "Short",
                SubCategoryId = Guid.NewGuid(),
                OwnerId = Guid.NewGuid(),
                PostSettings = new PostSettings
                {
                    Id = Guid.NewGuid(),
                    IsDeleted = false,
                    IsHidden = true,
                    postStatusType = PostStatusType.Published
                }
            }
        };

        var mockPosts = posts.AsQueryable().BuildMock();

        _postQueryRepositoryMock
            .Setup(x => x.GetAllPosts())
            .Returns(mockPosts);

        var query = new GetPostByIdQuery { PostId = postId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Null(result.Value);
        Assert.Equal("Post not found or is hidden/deleted.", result.Error);
        Assert.Equal(404, result.StatusCode);

        _mapperMock.Verify(x => x.Map<PostDto>(It.IsAny<Post>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenPostIsBothDeletedAndHidden_ReturnsFailure()
    {
        // Arrange
        var postId = Guid.NewGuid();

        var posts = new List<Post>
        {
            new CommonPost
            {
                Id = postId,
                Title = "Deleted and Hidden Post",
                FullDescription = "Description",
                ShortDescription = "Short",
                SubCategoryId = Guid.NewGuid(),
                OwnerId = Guid.NewGuid(),
                PostSettings = new PostSettings
                {
                    Id = Guid.NewGuid(),
                    IsDeleted = true,
                    IsHidden = true,
                    postStatusType = PostStatusType.Published
                }
            }
        };

        var mockPosts = posts.AsQueryable().BuildMock();

        _postQueryRepositoryMock
            .Setup(x => x.GetAllPosts())
            .Returns(mockPosts);

        var query = new GetPostByIdQuery { PostId = postId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Null(result.Value);
        Assert.Equal("Post not found or is hidden/deleted.", result.Error);
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task Handle_MapsPostDtoCorrectly()
    {
        // Arrange
        var postId = Guid.NewGuid();
        var ownerId = Guid.NewGuid();
        var subCategoryId = Guid.NewGuid();
        var title = "Mapped Post Title";
        var fullDesc = "Full mapped description";
        var shortDesc = "Short mapped description";

        var posts = new List<Post>
        {
            new CommonPost
            {
                Id = postId,
                Title = title,
                FullDescription = fullDesc,
                ShortDescription = shortDesc,
                SubCategoryId = subCategoryId,
                OwnerId = ownerId,
                PostSettings = new PostSettings
                {
                    Id = Guid.NewGuid(),
                    IsDeleted = false,
                    IsHidden = false,
                    postStatusType = PostStatusType.Published
                }
            }
        };

        var mockPosts = posts.AsQueryable().BuildMock();

        var expectedDto = new PostDto
        {
            Id = postId.ToString(),
            Title = title,
            OwnerId = ownerId.ToString(),
            SubCategoryId = subCategoryId.ToString(),
            FullDescription = fullDesc,
            ShortDescription = shortDesc
        };

        _postQueryRepositoryMock
            .Setup(x => x.GetAllPosts())
            .Returns(mockPosts);

        _mapperMock
            .Setup(x => x.Map<PostDto>(It.IsAny<Post>()))
            .Returns(expectedDto);

        var query = new GetPostByIdQuery { PostId = postId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedDto.Id, result.Value.Id);
        Assert.Equal(expectedDto.Title, result.Value.Title);
        Assert.Equal(expectedDto.OwnerId, result.Value.OwnerId);
        Assert.Equal(expectedDto.SubCategoryId, result.Value.SubCategoryId);
        Assert.Equal(expectedDto.FullDescription, result.Value.FullDescription);
        Assert.Equal(expectedDto.ShortDescription, result.Value.ShortDescription);
    }

    [Fact]
    public async Task Handle_WithDifferentPostIds_ReturnsCorrectPost()
    {
        // Arrange
        var postId1 = Guid.NewGuid();
        var postId2 = Guid.NewGuid();

        var posts = new List<Post>
        {
            new CommonPost
            {
                Id = postId1,
                Title = "Post 1",
                FullDescription = "Desc",
                ShortDescription = "Short",
                SubCategoryId = Guid.NewGuid(),
                OwnerId = Guid.NewGuid(),
                PostSettings = new PostSettings
                {
                    Id = Guid.NewGuid(),
                    IsDeleted = false,
                    IsHidden = false
                }
            },
            new CommonPost
            {
                Id = postId2,
                Title = "Post 2",
                FullDescription = "Desc 2",
                ShortDescription = "Short 2",
                SubCategoryId = Guid.NewGuid(),
                OwnerId = Guid.NewGuid(),
                PostSettings = new PostSettings
                {
                    Id = Guid.NewGuid(),
                    IsDeleted = false,
                    IsHidden = false
                }
            }
        };

        var mockPosts = posts.AsQueryable().BuildMock();

        _postQueryRepositoryMock
            .Setup(x => x.GetAllPosts())
            .Returns(mockPosts);

        _mapperMock
            .Setup(x => x.Map<PostDto>(It.IsAny<Post>()))
            .Returns((Post p) => new PostDto
            {
                Id = p.Id.ToString(),
                OwnerId = p.OwnerId.ToString(),
                SubCategoryId = p.SubCategoryId.ToString(),
                Title = p.Title,
                FullDescription = p.FullDescription,
                ShortDescription = p.ShortDescription
            });

        var query = new GetPostByIdQuery { PostId = postId1 };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(postId1.ToString(), result.Value.Id);
        Assert.Equal("Post 1", result.Value.Title);
    }

    [Fact]
    public async Task Handle_WhenPostIsPublishedButNotDeletedOrHidden_ReturnsSuccess()
    {
        // Arrange
        var postId = Guid.NewGuid();

        var posts = new List<Post>
        {
            new CommonPost
            {
                Id = postId,
                Title = "Published Post",
                FullDescription = "Description",
                ShortDescription = "Short",
                SubCategoryId = Guid.NewGuid(),
                OwnerId = Guid.NewGuid(),
                PostSettings = new PostSettings
                {
                    Id = Guid.NewGuid(),
                    IsDeleted = false,
                    IsHidden = false,
                    postStatusType = PostStatusType.Published
                }
            }
        };

        var mockPosts = posts.AsQueryable().BuildMock();

        var postDto = new PostDto
        {
            Id = postId.ToString(),
            Title = "Published Post",
            OwnerId = Guid.NewGuid().ToString(),
            SubCategoryId = Guid.NewGuid().ToString(),
            FullDescription = "Description",
            ShortDescription = "Short"
        };

        _postQueryRepositoryMock
            .Setup(x => x.GetAllPosts())
            .Returns(mockPosts);

        _mapperMock
            .Setup(x => x.Map<PostDto>(It.IsAny<Post>()))
            .Returns(postDto);

        var query = new GetPostByIdQuery { PostId = postId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task Handle_WhenPostNotFound_ReturnsFailure()
    {
        // Arrange
        var postId = Guid.NewGuid();
        var emptyPosts = new List<Post>();
        var mockPosts = emptyPosts.AsQueryable().BuildMock();

        _postQueryRepositoryMock
            .Setup(x => x.GetAllPosts())
            .Returns(mockPosts);

        var query = new GetPostByIdQuery { PostId = postId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Null(result.Value);
        Assert.Equal("Post not found or is hidden/deleted.", result.Error);
        Assert.Equal(404, result.StatusCode);
    }
}