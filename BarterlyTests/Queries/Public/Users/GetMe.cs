using Application.Core.MediatR.Requests;
using Application.Queries.Public.Users.GetMe;
using Domain.Entities.Users;
using Domain.Enums.Users;
using Domain.Interfaces.Queries.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace BarterlyUnitTests.Queries.Public.Users;

public class GetMe
{
    private readonly Mock<IUserQueryRepository> _userRepositoryMock;
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<IUserFavPostQueryRepository> _favPostRepositoryMock;
    private readonly Mock<INotificationQueryRepository> _notificationRepositoryMock;
    private readonly GetMeQueryHandler _handler;

    public GetMe()
    {
        _userRepositoryMock = new Mock<IUserQueryRepository>();
        var userStoreMock = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(
            userStoreMock.Object, null, null, null, null, null, null, null, null);
        _favPostRepositoryMock = new Mock<IUserFavPostQueryRepository>();
        _notificationRepositoryMock = new Mock<INotificationQueryRepository>();

        _handler = new GetMeQueryHandler(
            _favPostRepositoryMock.Object,
            _userRepositoryMock.Object,
            _userManagerMock.Object,
            _notificationRepositoryMock.Object
        );
    }

    private AuthorizeData CreateAuthorizeData(Guid userId, UserRoles role = UserRoles.User)
    {
        return new AuthorizeData(
            userId,
            new List<UserRoles> { role },
            "test-token",
            new Endpoint(null, null, "TestEndpoint") 
        );
    }

    [Fact]
    public async Task Handle_WhenUserExists_ReturnsSuccessWithUserData()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            ProfilePicturePath = "/images/profile.jpg"
        };

        var favPosts = new List<UserFavouritePost>
        {
            new UserFavouritePost() { UserId = userId, PostId = Guid.NewGuid() },
            new UserFavouritePost() { UserId = userId, PostId = Guid.NewGuid() }
        };

        var notifications = new List<Notification>
        {
            new Notification { Id = Guid.NewGuid(), UserId = userId, IsRead = false, Message = "", Title = ""},
            new Notification { Id = Guid.NewGuid(), UserId = userId, IsRead = false, Message = "", Title = ""},
            new Notification { Id = Guid.NewGuid(), UserId = userId, IsRead = true, Message = "", Title = ""}
        };

        _userRepositoryMock
            .Setup(x => x.GetUserAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _favPostRepositoryMock
            .Setup(x => x.GetUserFavPostsByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(favPosts);

        _notificationRepositoryMock
            .Setup(x => x.GetNotificationsByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(notifications);

        _userManagerMock
            .Setup(x => x.IsInRoleAsync(user, "Admin"))
            .ReturnsAsync(false);

        _userManagerMock
            .Setup(x => x.IsInRoleAsync(user, "Moderator"))
            .ReturnsAsync(false);

        var query = new GetMeQuery
        {
            AuthorizeData = CreateAuthorizeData(userId)
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(userId.ToString(), result.Value.Id);
        Assert.Equal("John", result.Value.FirstName);
        Assert.Equal("Doe", result.Value.LastName);
        Assert.Equal("john.doe@example.com", result.Value.Email);
        Assert.Equal("/images/profile.jpg", result.Value.ProfilePicturePath);
        Assert.Equal("User", result.Value.Role);
        Assert.Equal(2, result.Value.NotificationCount);
        Assert.Equal(2, result.Value.FavPostIds.Count);
    }

    [Fact]
    public async Task Handle_WhenUserIsAdmin_ReturnsAdminRole()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            FirstName = "Admin",
            LastName = "User",
            Email = "admin@example.com"
        };

        _userRepositoryMock
            .Setup(x => x.GetUserAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _favPostRepositoryMock
            .Setup(x => x.GetUserFavPostsByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<UserFavouritePost>());

        _notificationRepositoryMock
            .Setup(x => x.GetNotificationsByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Notification>());

        _userManagerMock
            .Setup(x => x.IsInRoleAsync(user, "Admin"))
            .ReturnsAsync(true);

        var query = new GetMeQuery
        {
            AuthorizeData = CreateAuthorizeData(userId, UserRoles.Admin)
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Admin", result.Value.Role);
    }

    [Fact]
    public async Task Handle_WhenUserIsModerator_ReturnsModeratorRole()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            FirstName = "Moderator",
            LastName = "User",
            Email = "mod@example.com"
        };

        _userRepositoryMock
            .Setup(x => x.GetUserAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _favPostRepositoryMock
            .Setup(x => x.GetUserFavPostsByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<UserFavouritePost>());

        _notificationRepositoryMock
            .Setup(x => x.GetNotificationsByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Notification>());

        _userManagerMock
            .Setup(x => x.IsInRoleAsync(user, "Admin"))
            .ReturnsAsync(false);

        _userManagerMock
            .Setup(x => x.IsInRoleAsync(user, "Moderator"))
            .ReturnsAsync(true);

        var query = new GetMeQuery
        {
            AuthorizeData = CreateAuthorizeData(userId, UserRoles.Moderator)
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Moderator", result.Value.Role);
    }

    [Fact]
    public async Task Handle_WhenUserHasNoFavoritePosts_ReturnsEmptyFavPostIds()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com"
        };

        _userRepositoryMock
            .Setup(x => x.GetUserAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _favPostRepositoryMock
            .Setup(x => x.GetUserFavPostsByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<UserFavouritePost>());

        _notificationRepositoryMock
            .Setup(x => x.GetNotificationsByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Notification>());

        _userManagerMock
            .Setup(x => x.IsInRoleAsync(user, It.IsAny<string>()))
            .ReturnsAsync(false);

        var query = new GetMeQuery
        {
            AuthorizeData = CreateAuthorizeData(userId)
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Value.FavPostIds);
    }

    [Fact]
    public async Task Handle_WhenUserHasNoNotifications_ReturnsZeroNotificationCount()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com"
        };

        _userRepositoryMock
            .Setup(x => x.GetUserAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _favPostRepositoryMock
            .Setup(x => x.GetUserFavPostsByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<UserFavouritePost>());

        _notificationRepositoryMock
            .Setup(x => x.GetNotificationsByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Notification>());

        _userManagerMock
            .Setup(x => x.IsInRoleAsync(user, It.IsAny<string>()))
            .ReturnsAsync(false);

        var query = new GetMeQuery
        {
            AuthorizeData = CreateAuthorizeData(userId)
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(0, result.Value.NotificationCount);
    }

    [Fact]
    public async Task Handle_WhenAllNotificationsAreRead_ReturnsZeroNotificationCount()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com"
        };

        var notifications = new List<Notification>
        {
            new Notification { Id = Guid.NewGuid(), UserId = userId, IsRead = true, Message = "", Title = "" },
            new Notification { Id = Guid.NewGuid(), UserId = userId, IsRead = true, Message = "", Title = ""}
        };

        _userRepositoryMock
            .Setup(x => x.GetUserAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _favPostRepositoryMock
            .Setup(x => x.GetUserFavPostsByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<UserFavouritePost>());

        _notificationRepositoryMock
            .Setup(x => x.GetNotificationsByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(notifications);

        _userManagerMock
            .Setup(x => x.IsInRoleAsync(user, It.IsAny<string>()))
            .ReturnsAsync(false);

        var query = new GetMeQuery
        {
            AuthorizeData = CreateAuthorizeData(userId)
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(0, result.Value.NotificationCount);
    }

    [Fact]
    public async Task Handle_CountsOnlyUnreadNotifications()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com"
        };

        var notifications = new List<Notification>
        {
            new Notification { Id = Guid.NewGuid(), UserId = userId, IsRead = false, Message = "", Title = "" },
            new Notification { Id = Guid.NewGuid(), UserId = userId, IsRead = false, Message = "", Title = "" },
            new Notification { Id = Guid.NewGuid(), UserId = userId, IsRead = false, Message = "", Title = "" },
            new Notification { Id = Guid.NewGuid(), UserId = userId, IsRead = true, Message = "", Title = "" },
            new Notification { Id = Guid.NewGuid(), UserId = userId, IsRead = true, Message = "", Title = "" }
        };

        _userRepositoryMock
            .Setup(x => x.GetUserAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _favPostRepositoryMock
            .Setup(x => x.GetUserFavPostsByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<UserFavouritePost>());

        _notificationRepositoryMock
            .Setup(x => x.GetNotificationsByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(notifications);

        _userManagerMock
            .Setup(x => x.IsInRoleAsync(user, It.IsAny<string>()))
            .ReturnsAsync(false);

        var query = new GetMeQuery
        {
            AuthorizeData = CreateAuthorizeData(userId)
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(3, result.Value.NotificationCount);
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
            ProfilePicturePath = null
        };

        _userRepositoryMock
            .Setup(x => x.GetUserAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _favPostRepositoryMock
            .Setup(x => x.GetUserFavPostsByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<UserFavouritePost>());

        _notificationRepositoryMock
            .Setup(x => x.GetNotificationsByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Notification>());

        _userManagerMock
            .Setup(x => x.IsInRoleAsync(user, It.IsAny<string>()))
            .ReturnsAsync(false);

        var query = new GetMeQuery
        {
            AuthorizeData = CreateAuthorizeData(userId)
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Null(result.Value.ProfilePicturePath);
    }

    [Fact]
    public async Task Handle_ReturnsFavPostIdsCorrectly()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var postId1 = Guid.NewGuid();
        var postId2 = Guid.NewGuid();
        var postId3 = Guid.NewGuid();

        var user = new User
        {
            Id = userId,
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com"
        };

        var favPosts = new List<UserFavouritePost>
        {
            new UserFavouritePost { UserId = userId, PostId = postId1 },
            new UserFavouritePost { UserId = userId, PostId = postId2 },
            new UserFavouritePost { UserId = userId, PostId = postId3 }
        };

        _userRepositoryMock
            .Setup(x => x.GetUserAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _favPostRepositoryMock
            .Setup(x => x.GetUserFavPostsByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(favPosts);

        _notificationRepositoryMock
            .Setup(x => x.GetNotificationsByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Notification>());

        _userManagerMock
            .Setup(x => x.IsInRoleAsync(user, It.IsAny<string>()))
            .ReturnsAsync(false);

        var query = new GetMeQuery
        {
            AuthorizeData = CreateAuthorizeData(userId)
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(3, result.Value.FavPostIds.Count);
        Assert.Contains(postId1, result.Value.FavPostIds);
        Assert.Contains(postId2, result.Value.FavPostIds);
        Assert.Contains(postId3, result.Value.FavPostIds);
    }
}