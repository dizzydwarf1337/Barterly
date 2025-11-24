using Application.DTOs.Posts;
using Application.Queries.Public.Users.GetUserById;
using AutoMapper;
using Domain.Entities.Posts;
using Domain.Entities.Posts.PostTypes;
using Domain.Entities.Users;
using Domain.Enums.Posts;
using Domain.Interfaces.Queries.User;
using MockQueryable;
using Moq;

namespace BarterlyUnitTests.Queries.Public.Users;

public class GetUserById
{
    private readonly Mock<IUserQueryRepository> _userQueryRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetUserByIdQueryHandler _handler;

    public GetUserById()
    {
        _userQueryRepositoryMock = new Mock<IUserQueryRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetUserByIdQueryHandler(
            _userQueryRepositoryMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_WhenUserExistsWithPosts_ReturnsSuccessWithUserData()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var post1Id = Guid.NewGuid();
        var post2Id = Guid.NewGuid();

        var posts = new List<Post>
        {
            CreatePost(post1Id, userId),
            CreatePost(post2Id, userId)
        };

        var user = new User
        {
            Id = userId,
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            ProfilePicturePath = "/images/profile.jpg",
            CreatedAt = DateTime.UtcNow,
            Setting = new UserSettings
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                IsHidden = false,
                IsBanned = false,
                IsDeleted = false
            },
            UserPosts = posts
        };

        var users = new List<User> { user };
        var mockUsers = users.AsQueryable().BuildMock();

        _userQueryRepositoryMock
            .Setup(x => x.GetUsers())
            .Returns(mockUsers);

        var postDtos = new List<PostPreviewDto>
        {
            new PostPreviewDto { Id = post1Id.ToString(), SubCategoryId = Guid.NewGuid().ToString() },
            new PostPreviewDto { Id = post2Id.ToString(), SubCategoryId = Guid.NewGuid().ToString() }
        };

        _mapperMock
            .Setup(x => x.Map<ICollection<PostPreviewDto>>(It.IsAny<IEnumerable<Post>>()))
            .Returns(postDtos);

        var query = new GetUserByIdQuery { Id = userId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(userId, result.Value.Id);
        Assert.Equal("John", result.Value.FirstName);
        Assert.Equal("Doe", result.Value.LastName);
        Assert.Equal("/images/profile.jpg", result.Value.ProfilePicturePath);
        Assert.Equal(2, result.Value.Posts.Count);
    }

    [Fact]
    public async Task Handle_WhenUserExistsWithoutPosts_ReturnsSuccessWithEmptyPosts()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var user = new User
        {
            Id = userId,
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            ProfilePicturePath = "/images/profile.jpg",
            CreatedAt = DateTime.UtcNow,
            Setting = new UserSettings
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                IsHidden = false,
                IsBanned = false,
                IsDeleted = false
            },
            UserPosts = new List<Post>()
        };

        var users = new List<User> { user };
        var mockUsers = users.AsQueryable().BuildMock();

        _userQueryRepositoryMock
            .Setup(x => x.GetUsers())
            .Returns(mockUsers);

        _mapperMock
            .Setup(x => x.Map<ICollection<PostPreviewDto>>(It.IsAny<IEnumerable<Post>>()))
            .Returns(new List<PostPreviewDto>());

        var query = new GetUserByIdQuery { Id = userId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Empty(result.Value.Posts);
    }

    [Fact]
    public async Task Handle_WhenUserNotFound_ReturnsFailure()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var emptyUsers = new List<User>();
        var mockUsers = emptyUsers.AsQueryable().BuildMock();

        _userQueryRepositoryMock
            .Setup(x => x.GetUsers())
            .Returns(mockUsers);

        var query = new GetUserByIdQuery { Id = userId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Nie znaleziono użytkownika", result.Error);
        Assert.Null(result.Value);
    }

    [Fact]
    public async Task Handle_WhenUserIsHidden_ReturnsFailure()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var user = new User
        {
            Id = userId,
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            CreatedAt = DateTime.UtcNow,
            Setting = new UserSettings
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                IsHidden = true,
                IsBanned = false,
                IsDeleted = false
            },
            UserPosts = new List<Post>()
        };

        var users = new List<User> { user };
        var mockUsers = users.AsQueryable().BuildMock();

        _userQueryRepositoryMock
            .Setup(x => x.GetUsers())
            .Returns(mockUsers);

        var query = new GetUserByIdQuery { Id = userId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Nie znaleziono użytkownika", result.Error);
    }

    [Fact]
    public async Task Handle_WhenUserIsBanned_ReturnsFailure()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var user = new User
        {
            Id = userId,
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            CreatedAt = DateTime.UtcNow,
            Setting = new UserSettings
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                IsHidden = false,
                IsBanned = true,
                IsDeleted = false
            },
            UserPosts = new List<Post>()
        };

        var users = new List<User> { user };
        var mockUsers = users.AsQueryable().BuildMock();

        _userQueryRepositoryMock
            .Setup(x => x.GetUsers())
            .Returns(mockUsers);

        var query = new GetUserByIdQuery { Id = userId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Nie znaleziono użytkownika", result.Error);
    }

    [Fact]
    public async Task Handle_WhenUserIsDeleted_ReturnsFailure()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var user = new User
        {
            Id = userId,
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            CreatedAt = DateTime.UtcNow,
            Setting = new UserSettings
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                IsHidden = false,
                IsBanned = false,
                IsDeleted = true
            },
            UserPosts = new List<Post>()
        };

        var users = new List<User> { user };
        var mockUsers = users.AsQueryable().BuildMock();

        _userQueryRepositoryMock
            .Setup(x => x.GetUsers())
            .Returns(mockUsers);

        var query = new GetUserByIdQuery { Id = userId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Nie znaleziono użytkownika", result.Error);
    }

    [Fact]
    public async Task Handle_ExcludesDeletedAndHiddenPosts()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var posts = new List<Post>
        {
            CreatePost(Guid.NewGuid(), userId, isDeleted: false, isHidden: false),
            CreatePost(Guid.NewGuid(), userId, isDeleted: true, isHidden: false),
            CreatePost(Guid.NewGuid(), userId, isDeleted: false, isHidden: true),
            CreatePost(Guid.NewGuid(), userId, isDeleted: false, isHidden: false)
        };

        var user = new User
        {
            Id = userId,
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            CreatedAt = DateTime.UtcNow,
            Setting = new UserSettings
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                IsHidden = false,
                IsBanned = false,
                IsDeleted = false
            },
            UserPosts = posts
        };

        var users = new List<User> { user };
        var mockUsers = users.AsQueryable().BuildMock();

        _userQueryRepositoryMock
            .Setup(x => x.GetUsers())
            .Returns(mockUsers);

        var postDtos = new List<PostPreviewDto>
        {
            new PostPreviewDto { Id = Guid.NewGuid().ToString(), SubCategoryId = Guid.NewGuid().ToString() },
            new PostPreviewDto { Id = Guid.NewGuid().ToString(), SubCategoryId = Guid.NewGuid().ToString() }
        };

        _mapperMock
            .Setup(x => x.Map<ICollection<PostPreviewDto>>(It.IsAny<IEnumerable<Post>>()))
            .Returns(postDtos);

        var query = new GetUserByIdQuery { Id = userId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.Posts.Count);
    }

    [Fact]
    public async Task Handle_MapsUserFieldsCorrectly()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var createdAt = DateTime.UtcNow.AddDays(-30);

        var user = new User
        {
            Id = userId,
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane@example.com",
            ProfilePicturePath = "/path/to/profile.jpg",
            CreatedAt = createdAt,
            Setting = new UserSettings
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                IsHidden = false,
                IsBanned = false,
                IsDeleted = false
            },
            UserPosts = new List<Post>()
        };

        var users = new List<User> { user };
        var mockUsers = users.AsQueryable().BuildMock();

        _userQueryRepositoryMock
            .Setup(x => x.GetUsers())
            .Returns(mockUsers);

        _mapperMock
            .Setup(x => x.Map<ICollection<PostPreviewDto>>(It.IsAny<IEnumerable<Post>>()))
            .Returns(new List<PostPreviewDto>());

        var query = new GetUserByIdQuery { Id = userId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(userId, result.Value.Id);
        Assert.Equal("Jane", result.Value.FirstName);
        Assert.Equal("Smith", result.Value.LastName);
        Assert.Equal("/path/to/profile.jpg", result.Value.ProfilePicturePath);
        Assert.Equal(createdAt, result.Value.CreatedAt);
    }

    [Fact]
    public async Task Handle_WhenUserHasNullProfilePicture_ReturnsNullProfilePicturePath()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var user = new User
        {
            Id = userId,
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            ProfilePicturePath = null,
            CreatedAt = DateTime.UtcNow,
            Setting = new UserSettings
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                IsHidden = false,
                IsBanned = false,
                IsDeleted = false
            },
            UserPosts = new List<Post>()
        };

        var users = new List<User> { user };
        var mockUsers = users.AsQueryable().BuildMock();

        _userQueryRepositoryMock
            .Setup(x => x.GetUsers())
            .Returns(mockUsers);

        _mapperMock
            .Setup(x => x.Map<ICollection<PostPreviewDto>>(It.IsAny<IEnumerable<Post>>()))
            .Returns(new List<PostPreviewDto>());

        var query = new GetUserByIdQuery { Id = userId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Null(result.Value.ProfilePicturePath);
    }

    [Fact]
    public async Task Handle_CallsMapperWithFilteredPosts()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var posts = new List<Post>
        {
            CreatePost(Guid.NewGuid(), userId, isDeleted: false, isHidden: false),
            CreatePost(Guid.NewGuid(), userId, isDeleted: true, isHidden: false)
        };

        var user = new User
        {
            Id = userId,
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            CreatedAt = DateTime.UtcNow,
            Setting = new UserSettings
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                IsHidden = false,
                IsBanned = false,
                IsDeleted = false
            },
            UserPosts = posts
        };

        var users = new List<User> { user };
        var mockUsers = users.AsQueryable().BuildMock();

        _userQueryRepositoryMock
            .Setup(x => x.GetUsers())
            .Returns(mockUsers);

        _mapperMock
            .Setup(x => x.Map<ICollection<PostPreviewDto>>(It.IsAny<IEnumerable<Post>>()))
            .Returns(new List<PostPreviewDto> 
            { 
                new PostPreviewDto { Id = Guid.NewGuid().ToString(), SubCategoryId = Guid.NewGuid().ToString() }
            });

        var query = new GetUserByIdQuery { Id = userId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _mapperMock.Verify(
            x => x.Map<ICollection<PostPreviewDto>>(It.IsAny<IEnumerable<Post>>()), 
            Times.Once);
    }

    // Helper methods
    private Post CreatePost(
        Guid postId, 
        Guid ownerId, 
        bool isDeleted = false, 
        bool isHidden = false)
    {
        return new CommonPost
        {
            Id = postId,
            Title = "Test Post",
            FullDescription = "Description",
            ShortDescription = "Short",
            SubCategoryId = Guid.NewGuid(),
            OwnerId = ownerId,
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