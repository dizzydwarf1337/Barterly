using Application.Core.MediatR.Requests;
using Application.Queries.Users.Notifications.MyNotifications;
using Domain.Entities.Users;
using Domain.Enums.Users;
using Domain.Interfaces.Queries.User;
using Moq;

namespace BarterlyUnitTests.Queries.Users.Notifications;

public class GetMyNotifications
{
    private readonly Mock<INotificationQueryRepository> _notificationQueryRepositoryMock;
    private readonly GetMyNotificationsQueryHandler _handler;

    public GetMyNotifications()
    {
        _notificationQueryRepositoryMock = new Mock<INotificationQueryRepository>();
        _handler = new GetMyNotificationsQueryHandler(_notificationQueryRepositoryMock.Object);
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
    public async Task Handle_WhenUserHasNotifications_ReturnsAllNotifications()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var notifications = new List<Notification>
        {
            new Notification
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Title = "Notification 1",
                Message = "Message 1",
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            },
            new Notification
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Title = "Notification 2",
                Message = "Message 2",
                IsRead = true,
                CreatedAt = DateTime.UtcNow
            }
        };

        _notificationQueryRepositoryMock
            .Setup(x => x.GetNotificationsByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(notifications);

        var query = new GetMyNotificationsQuery
        {
            AuthorizeData = CreateAuthorizeData(userId)
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(2, result.Value.Notifications.Count());
        
        _notificationQueryRepositoryMock.Verify(
            x => x.GetNotificationsByUserIdAsync(userId, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenUserHasNoNotifications_ReturnsEmptyCollection()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var emptyNotifications = new List<Notification>();

        _notificationQueryRepositoryMock
            .Setup(x => x.GetNotificationsByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(emptyNotifications);

        var query = new GetMyNotificationsQuery
        {
            AuthorizeData = CreateAuthorizeData(userId)
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Empty(result.Value.Notifications);
    }

    [Fact]
    public async Task Handle_CallsRepositoryWithCorrectUserId()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var notifications = new List<Notification>();

        _notificationQueryRepositoryMock
            .Setup(x => x.GetNotificationsByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(notifications);

        var query = new GetMyNotificationsQuery
        {
            AuthorizeData = CreateAuthorizeData(userId)
        };

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _notificationQueryRepositoryMock.Verify(
            x => x.GetNotificationsByUserIdAsync(userId, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ReturnsNotificationsInCorrectFormat()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var notificationId = Guid.NewGuid();
        var createdAt = DateTime.UtcNow;

        var notifications = new List<Notification>
        {
            new Notification
            {
                Id = notificationId,
                UserId = userId,
                Title = "Test Notification",
                Message = "Test Message",
                IsRead = false,
                CreatedAt = createdAt
            }
        };

        _notificationQueryRepositoryMock
            .Setup(x => x.GetNotificationsByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(notifications);

        var query = new GetMyNotificationsQuery
        {
            AuthorizeData = CreateAuthorizeData(userId)
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        var notification = result.Value.Notifications.First();
        Assert.Equal(notificationId, notification.Id);
        Assert.Equal(userId, notification.UserId);
        Assert.Equal("Test Notification", notification.Title);
        Assert.Equal("Test Message", notification.Message);
        Assert.False(notification.IsRead);
        Assert.Equal(createdAt, notification.CreatedAt);
    }

    [Fact]
    public async Task Handle_IncludesBothReadAndUnreadNotifications()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var notifications = new List<Notification>
        {
            new Notification
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Title = "Unread Notification",
                Message = "Message",
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            },
            new Notification
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Title = "Read Notification",
                Message = "Message",
                IsRead = true,
                CreatedAt = DateTime.UtcNow
            }
        };

        _notificationQueryRepositoryMock
            .Setup(x => x.GetNotificationsByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(notifications);

        var query = new GetMyNotificationsQuery
        {
            AuthorizeData = CreateAuthorizeData(userId)
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.Notifications.Count());
        Assert.Contains(result.Value.Notifications, n => n.IsRead == false);
        Assert.Contains(result.Value.Notifications, n => n.IsRead == true);
    }

    [Fact]
    public async Task Handle_AlwaysReturnsSuccess()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var notifications = new List<Notification>
        {
            new Notification
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Title = "Test",
                Message = "Test",
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            }
        };

        _notificationQueryRepositoryMock
            .Setup(x => x.GetNotificationsByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(notifications);

        var query = new GetMyNotificationsQuery
        {
            AuthorizeData = CreateAuthorizeData(userId)
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Null(result.Error);
    }

    [Fact]
    public async Task Handle_WithMultipleNotifications_ReturnsAllInOrder()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var notifications = new List<Notification>();
        for (int i = 1; i <= 5; i++)
        {
            notifications.Add(new Notification
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Title = $"Notification {i}",
                Message = $"Message {i}",
                IsRead = i % 2 == 0,
                CreatedAt = DateTime.UtcNow.AddMinutes(-i)
            });
        }

        _notificationQueryRepositoryMock
            .Setup(x => x.GetNotificationsByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(notifications);

        var query = new GetMyNotificationsQuery
        {
            AuthorizeData = CreateAuthorizeData(userId)
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(5, result.Value.Notifications.Count());
    }

    [Fact]
    public async Task Handle_PassesCancellationTokenToRepository()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var notifications = new List<Notification>();
        var cancellationToken = new CancellationToken();

        _notificationQueryRepositoryMock
            .Setup(x => x.GetNotificationsByUserIdAsync(userId, cancellationToken))
            .ReturnsAsync(notifications);

        var query = new GetMyNotificationsQuery
        {
            AuthorizeData = CreateAuthorizeData(userId)
        };

        // Act
        await _handler.Handle(query, cancellationToken);

        // Assert
        _notificationQueryRepositoryMock.Verify(
            x => x.GetNotificationsByUserIdAsync(userId, cancellationToken),
            Times.Once);
    }
}