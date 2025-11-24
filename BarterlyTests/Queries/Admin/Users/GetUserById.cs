using Application.Queries.Admins.Users.GetUserById;
using Domain.Entities.Users;
using Domain.Interfaces.Queries.User;
using MockQueryable;
using Moq;

namespace BarterlyUnitTests.Queries.Admin.Users;

public class GetUserByIdQueryHandlerTests
{
    private readonly Mock<IUserQueryRepository> _userQueryRepositoryMock;
    private readonly GetUserByIdQueryHandler _handler;

    public GetUserByIdQueryHandlerTests()
    {
        _userQueryRepositoryMock = new Mock<IUserQueryRepository>();
        _handler = new GetUserByIdQueryHandler(_userQueryRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WhenUserExists_ReturnsSuccessWithUserData()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var settingId = Guid.NewGuid();
        var createdAt = DateTime.UtcNow.AddDays(-30);
        var lastSeen = DateTime.UtcNow;

        var user = new User
        {
            Id = userId,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Bio = "Software developer",
            Country = "Poland",
            City = "Warsaw",
            Street = "Main Street",
            HouseNumber = "123",
            PostalCode = "00-001",
            ProfilePicturePath = "/images/profile.jpg",
            CreatedAt = createdAt,
            LastSeen = lastSeen,
            Setting = new UserSettings
            {
                Id = settingId,
                IsHidden = false,
                IsDeleted = false,
                IsBanned = false,
                IsPostRestricted = false,
                IsOpinionRestricted = false,
                IsChatRestricted = false
            }
        };

        var users = new List<User> { user }.AsQueryable().BuildMock();

        _userQueryRepositoryMock
            .Setup(x => x.GetUsers())
            .Returns(users);

        var query = new GetUserByIdQuery { Id = userId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        
        // Verify UserData
        Assert.Equal(userId, result.Value.UserData.Id);
        Assert.Equal("John", result.Value.UserData.FirstName);
        Assert.Equal("Doe", result.Value.UserData.LastName);
        Assert.Equal("john.doe@example.com", result.Value.UserData.Email);
        Assert.Equal("Software developer", result.Value.UserData.Bio);
        Assert.Equal("Poland", result.Value.UserData.Country);
        Assert.Equal("Warsaw", result.Value.UserData.City);
        Assert.Equal("Main Street", result.Value.UserData.Street);
        Assert.Equal("123", result.Value.UserData.HouseNumber);
        Assert.Equal("00-001", result.Value.UserData.PostalCode);
        Assert.Equal("/images/profile.jpg", result.Value.UserData.ProfilePicturePath);
        Assert.Equal(createdAt, result.Value.UserData.CreatedAt);
        Assert.Equal(lastSeen, result.Value.UserData.LastSeen);

        // Verify UserSettings
        Assert.Equal(settingId, result.Value.UserSettings.Id);
        Assert.False(result.Value.UserSettings.IsHidden);
        Assert.False(result.Value.UserSettings.IsDeleted);
        Assert.False(result.Value.UserSettings.IsBanned);
        Assert.False(result.Value.UserSettings.IsPostRestricted);
        Assert.False(result.Value.UserSettings.IsOpinionRestricted);
        Assert.False(result.Value.UserSettings.IsChatRestricted);

        _userQueryRepositoryMock.Verify(x => x.GetUsers(), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenUserDoesNotExist_ReturnsFailure()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var emptyUsers = new List<User>().AsQueryable().BuildMock();

        _userQueryRepositoryMock
            .Setup(x => x.GetUsers())
            .Returns(emptyUsers);

        var query = new GetUserByIdQuery { Id = userId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Null(result.Value);
        Assert.Equal("User not found", result.Error);

        _userQueryRepositoryMock.Verify(x => x.GetUsers(), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenUserHasNullOptionalFields_ReturnsSuccessWithNulls()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var user = new User
        {
            Id = userId,
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane.smith@example.com",
            Bio = null,
            Country = null,
            City = null,
            Street = null,
            HouseNumber = null,
            PostalCode = null,
            ProfilePicturePath = null,
            CreatedAt = DateTime.UtcNow,
            LastSeen = DateTime.UtcNow,
            Setting = new UserSettings
            {
                Id = Guid.NewGuid(),
                IsHidden = false,
                IsDeleted = false,
                IsBanned = false,
                IsPostRestricted = false,
                IsOpinionRestricted = false,
                IsChatRestricted = false
            }
        };

        var users = new List<User> { user }.AsQueryable().BuildMock();

        _userQueryRepositoryMock
            .Setup(x => x.GetUsers())
            .Returns(users);

        var query = new GetUserByIdQuery { Id = userId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Null(result.Value.UserData.Bio);
        Assert.Null(result.Value.UserData.Country);
        Assert.Null(result.Value.UserData.City);
        Assert.Null(result.Value.UserData.Street);
        Assert.Null(result.Value.UserData.HouseNumber);
        Assert.Null(result.Value.UserData.PostalCode);
        Assert.Null(result.Value.UserData.ProfilePicturePath);
    }

    [Fact]
    public async Task Handle_WhenUserIsRestricted_ReturnsCorrectSettings()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var user = new User
        {
            Id = userId,
            FirstName = "Restricted",
            LastName = "User",
            Email = "restricted@example.com",
            CreatedAt = DateTime.UtcNow,
            LastSeen = DateTime.UtcNow,
            Setting = new UserSettings
            {
                Id = Guid.NewGuid(),
                IsHidden = true,
                IsDeleted = false,
                IsBanned = true,
                IsPostRestricted = true,
                IsOpinionRestricted = true,
                IsChatRestricted = true
            }
        };

        var users = new List<User> { user }.AsQueryable().BuildMock();

        _userQueryRepositoryMock
            .Setup(x => x.GetUsers())
            .Returns(users);

        var query = new GetUserByIdQuery { Id = userId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.True(result.Value.UserSettings.IsHidden);
        Assert.True(result.Value.UserSettings.IsBanned);
        Assert.True(result.Value.UserSettings.IsPostRestricted);
        Assert.True(result.Value.UserSettings.IsOpinionRestricted);
        Assert.True(result.Value.UserSettings.IsChatRestricted);
    }

    [Fact]
    public async Task Handle_WhenUserIsDeleted_ReturnsCorrectSettings()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var user = new User
        {
            Id = userId,
            FirstName = "Deleted",
            LastName = "User",
            Email = "deleted@example.com",
            CreatedAt = DateTime.UtcNow,
            LastSeen = DateTime.UtcNow,
            Setting = new UserSettings
            {
                Id = Guid.NewGuid(),
                IsHidden = false,
                IsDeleted = true,
                IsBanned = false,
                IsPostRestricted = false,
                IsOpinionRestricted = false,
                IsChatRestricted = false
            }
        };

        var users = new List<User> { user }.AsQueryable().BuildMock();

        _userQueryRepositoryMock
            .Setup(x => x.GetUsers())
            .Returns(users);

        var query = new GetUserByIdQuery { Id = userId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.True(result.Value.UserSettings.IsDeleted);
        Assert.False(result.Value.UserSettings.IsHidden);
        Assert.False(result.Value.UserSettings.IsBanned);
    }

    [Fact]
    public async Task Handle_WhenMultipleUsersExist_ReturnsCorrectUser()
    {
        // Arrange
        var userId1 = Guid.NewGuid();
        var userId2 = Guid.NewGuid();
        var userId3 = Guid.NewGuid();

        var users = new List<User>
        {
            new User
            {
                Id = userId1,
                FirstName = "User1",
                LastName = "Test",
                Email = "user1@example.com",
                CreatedAt = DateTime.UtcNow,
                LastSeen = DateTime.UtcNow,
                Setting = new UserSettings { Id = Guid.NewGuid() }
            },
            new User
            {
                Id = userId2,
                FirstName = "User2",
                LastName = "Test",
                Email = "user2@example.com",
                CreatedAt = DateTime.UtcNow,
                LastSeen = DateTime.UtcNow,
                Setting = new UserSettings { Id = Guid.NewGuid() }
            },
            new User
            {
                Id = userId3,
                FirstName = "User3",
                LastName = "Test",
                Email = "user3@example.com",
                CreatedAt = DateTime.UtcNow,
                LastSeen = DateTime.UtcNow,
                Setting = new UserSettings { Id = Guid.NewGuid() }
            }
        }.AsQueryable().BuildMock();

        _userQueryRepositoryMock
            .Setup(x => x.GetUsers())
            .Returns(users);

        var query = new GetUserByIdQuery { Id = userId2 };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(userId2, result.Value.UserData.Id);
        Assert.Equal("User2", result.Value.UserData.FirstName);
        Assert.Equal("user2@example.com", result.Value.UserData.Email);
    }
}