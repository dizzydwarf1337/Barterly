using Application.Commands.Admins.Posts.RejectPost;
using Application.Core.MediatR.Requests;
using Application.Events.Posts.PostRejectedEvent;
using Application.Interfaces;
using Domain.Enums.Common;
using Domain.Enums.Posts;
using Domain.Enums.Users;
using Domain.Interfaces.Commands.Post;
using MediatR;
using Moq;

namespace BarterlyUnitTests.Commands.Admins.Posts;

public class RejectPost
{
    private readonly Mock<IPostSettingsCommandRepository> _postSettingsCommandRepositoryMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<ILogService> _logServiceMock;
    private readonly RejectPostCommandHandler _handler;

    public RejectPost()
    {
        _postSettingsCommandRepositoryMock = new Mock<IPostSettingsCommandRepository>();
        _mediatorMock = new Mock<IMediator>();
        _logServiceMock = new Mock<ILogService>();
        _handler = new RejectPostCommandHandler(
            _postSettingsCommandRepositoryMock.Object,
            _mediatorMock.Object,
            _logServiceMock.Object
        );
    }

    private AuthorizeData CreateAuthorizeData(Guid userId)
    {
        return new AuthorizeData(
            userId,
            new List<UserRoles> { UserRoles.Admin },
            "test-token",
            null
        );
    }

    [Fact]
    public async Task Handle_WhenValidCommand_RejectsPost()
    {
        // Arrange
        var postId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var reason = "Inappropriate content";

        var command = new RejectPostCommand
        {
            PostId = postId,
            Reason = reason,
            AuthorizeData = CreateAuthorizeData(userId)
        };

        _postSettingsCommandRepositoryMock
            .Setup(x => x.UpdatePostSettings(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<PostStatusType>(),
                It.IsAny<string?>()))
            .Returns(() => Task.CompletedTask);

        _mediatorMock
            .Setup(x => x.Publish(It.IsAny<PostRejectedEvent>(), It.IsAny<CancellationToken>()))
            .Returns(() => Task.CompletedTask);

        _logServiceMock
            .Setup(x => x.CreateLogAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<LogType?>(),
                It.IsAny<string?>(),
                It.IsAny<Guid?>(),
                It.IsAny<Guid?>()))
            .Returns(() => Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_UpdatesPostSettingsWithCorrectParameters()
    {
        // Arrange
        var postId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var reason = "Spam content";

        var command = new RejectPostCommand
        {
            PostId = postId,
            Reason = reason,
            AuthorizeData = CreateAuthorizeData(userId)
        };

        _postSettingsCommandRepositoryMock
            .Setup(x => x.UpdatePostSettings(
                postId,
                It.IsAny<CancellationToken>(),
                true,
                false,
                PostStatusType.Rejected,
                reason))
            .Returns(() => Task.CompletedTask);

        _mediatorMock
            .Setup(x => x.Publish(It.IsAny<PostRejectedEvent>(), It.IsAny<CancellationToken>()))
            .Returns(() => Task.CompletedTask);

        _logServiceMock
            .Setup(x => x.CreateLogAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<LogType?>(),
                It.IsAny<string?>(),
                It.IsAny<Guid?>(),
                It.IsAny<Guid?>()))
            .Returns(() => Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _postSettingsCommandRepositoryMock.Verify(
            x => x.UpdatePostSettings(
                postId,
                It.IsAny<CancellationToken>(),
                true,
                false,
                PostStatusType.Rejected,
                reason),
            Times.Once);
    }

    [Fact]
    public async Task Handle_PublishesPostRejectedEventWithCorrectData()
    {
        // Arrange
        var postId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var reason = "Policy violation";

        var command = new RejectPostCommand
        {
            PostId = postId,
            Reason = reason,
            AuthorizeData = CreateAuthorizeData(userId)
        };

        _postSettingsCommandRepositoryMock
            .Setup(x => x.UpdatePostSettings(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<PostStatusType>(),
                It.IsAny<string?>()))
            .Returns(() => Task.CompletedTask);

        _mediatorMock
            .Setup(x => x.Publish(It.IsAny<PostRejectedEvent>(), It.IsAny<CancellationToken>()))
            .Returns(() => Task.CompletedTask);

        _logServiceMock
            .Setup(x => x.CreateLogAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<LogType?>(),
                It.IsAny<string?>(),
                It.IsAny<Guid?>(),
                It.IsAny<Guid?>()))
            .Returns(() => Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mediatorMock.Verify(
            x => x.Publish(
                It.Is<PostRejectedEvent>(e => e.postId == postId && e.reason == reason),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_CreatesLogWithCorrectInformation()
    {
        // Arrange
        var postId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var reason = "Duplicate content";

        var command = new RejectPostCommand
        {
            PostId = postId,
            Reason = reason,
            AuthorizeData = CreateAuthorizeData(userId)
        };

        _postSettingsCommandRepositoryMock
            .Setup(x => x.UpdatePostSettings(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<PostStatusType>(),
                It.IsAny<string?>()))
            .Returns(() => Task.CompletedTask);

        _mediatorMock
            .Setup(x => x.Publish(It.IsAny<PostRejectedEvent>(), It.IsAny<CancellationToken>()))
            .Returns(() => Task.CompletedTask);

        _logServiceMock
            .Setup(x => x.CreateLogAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<LogType?>(),
                It.IsAny<string?>(),
                It.IsAny<Guid?>(),
                It.IsAny<Guid?>()))
            .Returns(() => Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _logServiceMock.Verify(
            x => x.CreateLogAsync(
                It.Is<string>(msg => msg.Contains(postId.ToString()) && msg.Contains("Post rejected")),
                It.IsAny<CancellationToken>(),
                LogType.Information,
                It.IsAny<string?>(),
                postId,
                userId),
            Times.Once);
    }

    [Fact]
    public async Task Handle_PassesCancellationTokenToRepository()
    {
        // Arrange
        var postId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var reason = "Test reason";

        var command = new RejectPostCommand
        {
            PostId = postId,
            Reason = reason,
            AuthorizeData = CreateAuthorizeData(userId)
        };

        var cancellationToken = new CancellationToken();

        _postSettingsCommandRepositoryMock
            .Setup(x => x.UpdatePostSettings(
                postId,
                cancellationToken,
                true,
                false,
                PostStatusType.Rejected,
                reason))
            .Returns(() => Task.CompletedTask);

        _mediatorMock
            .Setup(x => x.Publish(It.IsAny<PostRejectedEvent>(), It.IsAny<CancellationToken>()))
            .Returns(() => Task.CompletedTask);

        _logServiceMock
            .Setup(x => x.CreateLogAsync(
                It.IsAny<string>(),
                cancellationToken,
                It.IsAny<LogType?>(),
                It.IsAny<string?>(),
                It.IsAny<Guid?>(),
                It.IsAny<Guid?>()))
            .Returns(() => Task.CompletedTask);

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        _postSettingsCommandRepositoryMock.Verify(
            x => x.UpdatePostSettings(postId, cancellationToken, true, false, PostStatusType.Rejected, reason),
            Times.Once);
        _logServiceMock.Verify(
            x => x.CreateLogAsync(It.IsAny<string>(), cancellationToken, It.IsAny<LogType?>(), It.IsAny<string?>(), It.IsAny<Guid?>(), It.IsAny<Guid?>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ReturnsSuccessResponse()
    {
        // Arrange
        var postId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var reason = "Test reason";

        var command = new RejectPostCommand
        {
            PostId = postId,
            Reason = reason,
            AuthorizeData = CreateAuthorizeData(userId)
        };

        _postSettingsCommandRepositoryMock
            .Setup(x => x.UpdatePostSettings(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<PostStatusType>(),
                It.IsAny<string?>()))
            .Returns(() => Task.CompletedTask);

        _mediatorMock
            .Setup(x => x.Publish(It.IsAny<PostRejectedEvent>(), It.IsAny<CancellationToken>()))
            .Returns(() => Task.CompletedTask);

        _logServiceMock
            .Setup(x => x.CreateLogAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<LogType?>(),
                It.IsAny<string?>(),
                It.IsAny<Guid?>(),
                It.IsAny<Guid?>()))
            .Returns(() => Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_ExecutesOperationsInCorrectOrder()
    {
        // Arrange
        var postId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var reason = "Order test";

        var command = new RejectPostCommand
        {
            PostId = postId,
            Reason = reason,
            AuthorizeData = CreateAuthorizeData(userId)
        };

        var callOrder = new List<string>();

        _postSettingsCommandRepositoryMock
            .Setup(x => x.UpdatePostSettings(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<PostStatusType>(),
                It.IsAny<string?>()))
            .Callback(() => callOrder.Add("UpdateSettings"))
            .Returns(() => Task.CompletedTask);

        _mediatorMock
            .Setup(x => x.Publish(It.IsAny<PostRejectedEvent>(), It.IsAny<CancellationToken>()))
            .Callback(() => callOrder.Add("PublishEvent"))
            .Returns(() => Task.CompletedTask);

        _logServiceMock
            .Setup(x => x.CreateLogAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<LogType?>(),
                It.IsAny<string?>(),
                It.IsAny<Guid?>(),
                It.IsAny<Guid?>()))
            .Callback(() => callOrder.Add("CreateLog"))
            .Returns(() => Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(3, callOrder.Count);
        Assert.Equal("UpdateSettings", callOrder[0]);
        Assert.Equal("PublishEvent", callOrder[1]);
        Assert.Equal("CreateLog", callOrder[2]);
    }

    [Fact]
    public async Task Handle_WithDifferentReasons_PassesCorrectReason()
    {
        // Arrange
        var postId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var reason1 = "Offensive content";
        var reason2 = "Copyright violation";

        var command = new RejectPostCommand
        {
            PostId = postId,
            Reason = reason1,
            AuthorizeData = CreateAuthorizeData(userId)
        };

        _postSettingsCommandRepositoryMock
            .Setup(x => x.UpdatePostSettings(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<PostStatusType>(),
                It.IsAny<string?>()))
            .Returns(() => Task.CompletedTask);

        _mediatorMock
            .Setup(x => x.Publish(It.IsAny<PostRejectedEvent>(), It.IsAny<CancellationToken>()))
            .Returns(() => Task.CompletedTask);

        _logServiceMock
            .Setup(x => x.CreateLogAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<LogType?>(),
                It.IsAny<string?>(),
                It.IsAny<Guid?>(),
                It.IsAny<Guid?>()))
            .Returns(() => Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _postSettingsCommandRepositoryMock.Verify(
            x => x.UpdatePostSettings(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<PostStatusType>(),
                reason1),
            Times.Once);
        _postSettingsCommandRepositoryMock.Verify(
            x => x.UpdatePostSettings(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<PostStatusType>(),
                reason2),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WithDifferentPostIds_UpdatesCorrectPost()
    {
        // Arrange
        var postId1 = Guid.NewGuid();
        var postId2 = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var reason = "Test";

        var command = new RejectPostCommand
        {
            PostId = postId1,
            Reason = reason,
            AuthorizeData = CreateAuthorizeData(userId)
        };

        _postSettingsCommandRepositoryMock
            .Setup(x => x.UpdatePostSettings(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<PostStatusType>(),
                It.IsAny<string?>()))
            .Returns(() => Task.CompletedTask);

        _mediatorMock
            .Setup(x => x.Publish(It.IsAny<PostRejectedEvent>(), It.IsAny<CancellationToken>()))
            .Returns(() => Task.CompletedTask);

        _logServiceMock
            .Setup(x => x.CreateLogAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<LogType?>(),
                It.IsAny<string?>(),
                It.IsAny<Guid?>(),
                It.IsAny<Guid?>()))
            .Returns(() => Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _postSettingsCommandRepositoryMock.Verify(
            x => x.UpdatePostSettings(postId1, It.IsAny<CancellationToken>(), true, false, PostStatusType.Rejected, reason),
            Times.Once);
        _postSettingsCommandRepositoryMock.Verify(
            x => x.UpdatePostSettings(postId2, It.IsAny<CancellationToken>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<PostStatusType>(), It.IsAny<string?>()),
            Times.Never);
    }
}