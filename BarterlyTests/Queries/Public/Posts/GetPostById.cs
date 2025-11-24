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

namespace BarterlyUnitTests.Queries.Public.Posts;

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
    private Post CreatePost(Guid postId, bool isDeleted = false, bool isHidden = false)
    {
        return new CommonPost
        {
            Id = postId,
            Title = "Test Post",
            FullDescription = "Full description",
            ShortDescription = "Short description",
            SubCategoryId = Guid.NewGuid(),
            OwnerId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            Owner = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Email = "john@test.com"
            },
            PostSettings = new PostSettings
            {
                Id = Guid.NewGuid(),
                PostId = postId,
                IsDeleted = isDeleted,
                IsHidden = isHidden,
                postStatusType = PostStatusType.Published
            }
        };
    }

    [Fact]
    public async Task Handle_WhenPostExistsAndNotDeletedOrHidden_ReturnsSuccess()
    {
        // Arrange
        var postId = Guid.NewGuid();
        var posts = new List<Post> { CreatePost(postId) };
        var mockPosts = posts.BuildMock();

        var postDto = new PostDto
        {
            Id = postId.ToString(),
            Title = "Test Post",
            OwnerId = Guid.NewGuid().ToString(),
            SubCategoryId = Guid.NewGuid().ToString(),
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
        var posts = new List<Post> { CreatePost(postId, isDeleted: true) };
        var mockPosts = posts.BuildMock();

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
        var posts = new List<Post> { CreatePost(postId, isHidden: true) };
        var mockPosts = posts.BuildMock();

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
        var posts = new List<Post> { CreatePost(postId, isDeleted: true, isHidden: true) };
        var mockPosts = posts.BuildMock();

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
        
        var post = new CommonPost
        {
            Id = postId,
            Title = "Mapped Post Title",
            FullDescription = "Full mapped description",
            ShortDescription = "Short mapped description",
            SubCategoryId = subCategoryId,
            OwnerId = ownerId,
            CreatedAt = DateTime.UtcNow,
            Owner = new User { Id = ownerId, FirstName = "John", LastName = "Doe", Email = "john@test.com" },
            PostSettings = new PostSettings
            {
                Id = Guid.NewGuid(),
                PostId = postId,
                IsDeleted = false,
                IsHidden = false,
                postStatusType = PostStatusType.Published
            }
        };

        var posts = new List<Post> { post };
        var mockPosts = posts.BuildMock();

        var expectedDto = new PostDto
        {
            Id = postId.ToString(),
            Title = "Mapped Post Title",
            OwnerId = ownerId.ToString(),
            SubCategoryId = subCategoryId.ToString(),
            FullDescription = "Full mapped description",
            ShortDescription = "Short mapped description"
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
            CreatePost(postId1),
            CreatePost(postId2)
        };
        var mockPosts = posts.BuildMock();

        _postQueryRepositoryMock
            .Setup(x => x.GetAllPosts())
            .Returns(mockPosts);

        _mapperMock
            .Setup(x => x.Map<PostDto>(It.IsAny<Post>()))
            .Returns((Post p) => new PostDto
            {
                Id = p.Id.ToString(),
                Title = p.Title,
                OwnerId = p.OwnerId.ToString(),
                SubCategoryId = p.SubCategoryId.ToString(),
                FullDescription = p.FullDescription,
                ShortDescription = p.ShortDescription
            });

        var query = new GetPostByIdQuery { PostId = postId1 };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(postId1.ToString(), result.Value.Id);
    }

    [Fact]
    public async Task Handle_WhenPostIsPublishedButNotDeletedOrHidden_ReturnsSuccess()
    {
        // Arrange
        var postId = Guid.NewGuid();
        var posts = new List<Post> { CreatePost(postId) };
        var mockPosts = posts.BuildMock();

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
        var mockPosts = emptyPosts.BuildMock();

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