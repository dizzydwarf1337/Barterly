using Application.DTOs.Posts;
using Application.Queries.Admins.Posts.GetPostById;
using AutoMapper;
using Domain.Entities.Posts;
using Domain.Entities.Posts.PostTypes;
using Domain.Entities.Users;
using Domain.Interfaces.Queries.Post;
using MockQueryable.Moq;
using MediatR;
using MockQueryable;
using Moq;


namespace BarterlyUnitTests.Queries.Admin.Posts;

public class GetPostByIdQueryHandlerTests
{
    private readonly Mock<IPostQueryRepository> _postQueryRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly GetPostByIdQueryHandler _handler;

    public GetPostByIdQueryHandlerTests()
    {
        _postQueryRepositoryMock = new Mock<IPostQueryRepository>();
        _mapperMock = new Mock<IMapper>();
        _mediatorMock = new Mock<IMediator>();
        _handler = new GetPostByIdQueryHandler(
            _postQueryRepositoryMock.Object,
            _mapperMock.Object,
            _mediatorMock.Object
        );
    }

    [Fact]
    public async Task Handle_WhenPostExists_ReturnsSuccessWithMappedData()
    {
        // Arrange
        var postId = Guid.NewGuid();
        var ownerId = Guid.NewGuid();
        var subCategoryId = Guid.NewGuid();
        
        var owner = new User
        {
            Id = ownerId,
            FirstName = "John",
            LastName = "Doe"
        };

        var post = new CommonPost
        {
            Id = postId,
            OwnerId = ownerId,
            Owner = owner,
            Title = "Test Post",
            FullDescription = "Test full description",
            ShortDescription = "Test short description",
            SubCategoryId = subCategoryId,
            PostImages = new List<PostImage>
            {
                new PostImage 
                { 
                    Id = Guid.NewGuid(),
                    ImageUrl = "https://example.com/image.jpg"
                }
            },
            PostSettings = new PostSettings { Id = Guid.NewGuid() },
            PostOpinions = new List<PostOpinion>
            {
                new PostOpinion 
                { 
                    Id = Guid.NewGuid(),
                    Content = "Great post!"
                },
                new PostOpinion 
                { 
                    Id = Guid.NewGuid(),
                    Content = "Nice item!"
                }
            }
        };

        var posts = new List<Post> { post };
        var mockPosts = posts.BuildMock();

        var postDto = new PostDto 
        { 
            Id = postId.ToString(),
            OwnerId = ownerId.ToString(),
            Title = "Test Post",
            SubCategoryId = subCategoryId.ToString(),
            FullDescription = "Test full description",
            ShortDescription = "Test short description"
        };
        
        var settingsDto = new PostSettingsDto 
        { 
            Id = Guid.NewGuid().ToString()
        };
        
        var opinionsDto = new[]
        {
            new PostOpinionDto
            {
                Id = Guid.NewGuid().ToString(),
                AuthorId = Guid.NewGuid().ToString(),
                Content = "Great post!"
            },
            new PostOpinionDto
            {
                Id = Guid.NewGuid().ToString(),
                AuthorId = Guid.NewGuid().ToString(),
                Content = "Nice item!"
            }
        };

        _postQueryRepositoryMock
            .Setup(x => x.GetAllPosts())
            .Returns(mockPosts);

        _mapperMock
            .Setup(x => x.Map<PostDto>(It.IsAny<Post>()))
            .Returns(postDto);

        _mapperMock
            .Setup(x => x.Map<PostSettingsDto>(It.IsAny<PostSettings>()))
            .Returns(settingsDto);

        _mapperMock
            .Setup(x => x.Map<PostOpinionDto[]>(It.IsAny<List<PostOpinion>>()))
            .Returns(opinionsDto);

        var query = new GetPostByIdQuery { PostId = postId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(postDto, result.Value.Post);
        Assert.Equal(settingsDto, result.Value.Settings);
        Assert.Equal(opinionsDto, result.Value.Opinions);
        Assert.Equal(ownerId, result.Value.Owner.OwnerId);
        Assert.Equal("John", result.Value.Owner.FirstName);
        Assert.Equal("Doe", result.Value.Owner.LastName);

        _postQueryRepositoryMock.Verify(x => x.GetAllPosts(), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenPostDoesNotExist_ReturnsFailure()
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
        Assert.Equal("Post not found", result.Error);

        _postQueryRepositoryMock.Verify(x => x.GetAllPosts(), Times.Once);
        _mapperMock.Verify(x => x.Map<PostDto>(It.IsAny<Post>()), Times.Never);
    }

    [Fact]
    public async Task Handle_VerifiesCorrectPostIdIsQueried()
    {
        // Arrange
        var postId = Guid.NewGuid();
        var wrongPostId = Guid.NewGuid();
        var subCategoryId = Guid.NewGuid();
        
        var wrongPost = new CommonPost
        {
            Id = wrongPostId,
            OwnerId = Guid.NewGuid(),
            Owner = new User { Id = Guid.NewGuid(), FirstName = "Wrong", LastName = "User" },
            Title = "Wrong Post",
            FullDescription = "Wrong description",
            ShortDescription = "Wrong short",
            SubCategoryId = subCategoryId,
            PostImages = new List<PostImage>(),
            PostSettings = new PostSettings { Id = Guid.NewGuid() },
            PostOpinions = new List<PostOpinion>()
        };

        var posts = new List<Post> { wrongPost };
        var mockPosts = posts.BuildMock();

        _postQueryRepositoryMock
            .Setup(x => x.GetAllPosts())
            .Returns(mockPosts);

        var query = new GetPostByIdQuery { PostId = postId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Post not found", result.Error);
    }
}