using Application.Commands.Admins.Posts.DeletePost;
using Application.Core.MediatR.Requests;
using Application.Events.Posts.PostDeletedEvent;
using Application.Interfaces;
using Domain.Entities.Posts;
using Domain.Enums.Common;
using Domain.Enums.Posts;
using Domain.Enums.Users;
using Domain.Interfaces.Commands.Post;
using Domain.Interfaces.Queries.Post;
using MediatR;
using Moq;

namespace BarterlyUnitTests.Commands.Admins.Posts;

public class DeletePost
{
    private readonly Mock<IPostSettingsCommandRepository> _postSettingsCommandRepositoryMock;
    private readonly Mock<IPostSettingsQueryRepository> _postSettingsQueryRepositoryMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<ILogService> _logServiceMock;
    private readonly DeletePostCommandHandler _handler;

    public DeletePost()
    {
        _postSettingsCommandRepositoryMock = new Mock<IPostSettingsCommandRepository>();
        _postSettingsQueryRepositoryMock = new Mock<IPostSettingsQueryRepository>();
        _mediatorMock = new Mock<IMediator>();
        _logServiceMock = new Mock<ILogService>();
        _handler = new DeletePostCommandHandler(
            _postSettingsCommandRepositoryMock.Object,
            _postSettingsQueryRepositoryMock.Object,
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
    public async Task Handle_WhenValidCommand_DeletesPost()
    {
        // Arrange
        var postId = Guid.NewGuid();
        var settingsId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var command = new DeletePostCommand
        {
            PostId = postId,
            AuthorizeData = CreateAuthorizeData(userId)
        };

        var postSettings = new PostSettings
        {
            Id = settingsId,
            PostId = postId
        };

        _postSettingsQueryRepositoryMock
            .Setup(x => x.GetPostSettingsByPostId(postId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(postSettings);

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
            .Setup(x => x.Publish(It.IsAny<PostDeletedEvent>(), It.IsAny<CancellationToken>()))
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
    public async Task Handle_CallsGetPostSettingsByPostIdWithCorrectPostId()
    {
        // Arrange
        var postId = Guid.NewGuid();
        var settingsId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var command = new DeletePostCommand
        {
            PostId = postId,
            AuthorizeData = CreateAuthorizeData(userId)
        };

        var postSettings = new PostSettings
        {
            Id = settingsId,
            PostId = postId
        };

        _postSettingsQueryRepositoryMock
            .Setup(x => x.GetPostSettingsByPostId(postId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(postSettings);

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
            .Setup(x => x.Publish(It.IsAny<PostDeletedEvent>(), It.IsAny<CancellationToken>()))
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
        _postSettingsQueryRepositoryMock.Verify(
            x => x.GetPostSettingsByPostId(postId, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_UpdatesPostSettingsWithCorrectParameters()
    {
        // Arrange
        var postId = Guid.NewGuid();
        var settingsId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var command = new DeletePostCommand
        {
            PostId = postId,
            AuthorizeData = CreateAuthorizeData(userId)
        };

        var postSettings = new PostSettings
        {
            Id = settingsId,
            PostId = postId
        };

        _postSettingsQueryRepositoryMock
            .Setup(x => x.GetPostSettingsByPostId(postId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(postSettings);

        _postSettingsCommandRepositoryMock
            .Setup(x => x.UpdatePostSettings(
                settingsId,
                It.IsAny<CancellationToken>(),
                false,
                true,
                PostStatusType.Deleted,
                null))
            .Returns(() => Task.CompletedTask);

        _mediatorMock
            .Setup(x => x.Publish(It.IsAny<PostDeletedEvent>(), It.IsAny<CancellationToken>()))
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
                settingsId,
                It.IsAny<CancellationToken>(),
                false,
                true,
                PostStatusType.Deleted,
                null),
            Times.Once);
    }

    [Fact]
    public async Task Handle_PublishesPostDeletedEvent()
    {
        // Arrange
        var postId = Guid.NewGuid();
        var settingsId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var command = new DeletePostCommand
        {
            PostId = postId,
            AuthorizeData = CreateAuthorizeData(userId)
        };

        var postSettings = new PostSettings
        {
            Id = settingsId,
            PostId = postId
        };

        _postSettingsQueryRepositoryMock
            .Setup(x => x.GetPostSettingsByPostId(postId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(postSettings);

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
            .Setup(x => x.Publish(It.IsAny<PostDeletedEvent>(), It.IsAny<CancellationToken>()))
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
                It.Is<PostDeletedEvent>(e => e.postId == postId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_CreatesLogWithCorrectInformation()
    {
        // Arrange
        var postId = Guid.NewGuid();
        var settingsId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var command = new DeletePostCommand
        {
            PostId = postId,
            AuthorizeData = CreateAuthorizeData(userId)
        };

        var postSettings = new PostSettings
        {
            Id = settingsId,
            PostId = postId
        };

        _postSettingsQueryRepositoryMock
            .Setup(x => x.GetPostSettingsByPostId(postId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(postSettings);

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
            .Setup(x => x.Publish(It.IsAny<PostDeletedEvent>(), It.IsAny<CancellationToken>()))
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
                It.Is<string>(msg => msg.Contains(postId.ToString()) && msg.Contains("Post deleted")),
                It.IsAny<CancellationToken>(),
                LogType.Information,
                It.IsAny<string?>(),
                postId,
                userId),
            Times.Once);
    }

    [Fact]
    public async Task Handle_PassesCancellationTokenToRepositories()
    {
        // Arrange
        var postId = Guid.NewGuid();
        var settingsId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var command = new DeletePostCommand
        {
            PostId = postId,
            AuthorizeData = CreateAuthorizeData(userId)
        };

        var postSettings = new PostSettings
        {
            Id = settingsId,
            PostId = postId
        };

        var cancellationToken = new CancellationToken();

        _postSettingsQueryRepositoryMock
            .Setup(x => x.GetPostSettingsByPostId(postId, cancellationToken))
            .ReturnsAsync(postSettings);

        _postSettingsCommandRepositoryMock
            .Setup(x => x.UpdatePostSettings(
                settingsId,
                cancellationToken,
                false,
                true,
                PostStatusType.Deleted,
                null))
            .Returns(() => Task.CompletedTask);

        _mediatorMock
            .Setup(x => x.Publish(It.IsAny<PostDeletedEvent>(), It.IsAny<CancellationToken>()))
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
        _postSettingsQueryRepositoryMock.Verify(
            x => x.GetPostSettingsByPostId(postId, cancellationToken),
            Times.Once);
        _postSettingsCommandRepositoryMock.Verify(
            x => x.UpdatePostSettings(settingsId, cancellationToken, false, true, PostStatusType.Deleted, null),
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
        var settingsId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var command = new DeletePostCommand
        {
            PostId = postId,
            AuthorizeData = CreateAuthorizeData(userId)
        };

        var postSettings = new PostSettings
        {
            Id = settingsId,
            PostId = postId
        };

        _postSettingsQueryRepositoryMock
            .Setup(x => x.GetPostSettingsByPostId(postId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(postSettings);

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
            .Setup(x => x.Publish(It.IsAny<PostDeletedEvent>(), It.IsAny<CancellationToken>()))
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
        var settingsId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var command = new DeletePostCommand
        {
            PostId = postId,
            AuthorizeData = CreateAuthorizeData(userId)
        };

        var postSettings = new PostSettings
        {
            Id = settingsId,
            PostId = postId
        };

        var callOrder = new List<string>();

        _postSettingsQueryRepositoryMock
            .Setup(x => x.GetPostSettingsByPostId(postId, It.IsAny<CancellationToken>()))
            .Callback(() => callOrder.Add("GetSettings"))
            .ReturnsAsync(postSettings);

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
            .Setup(x => x.Publish(It.IsAny<PostDeletedEvent>(), It.IsAny<CancellationToken>()))
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
        Assert.Equal(4, callOrder.Count);
        Assert.Equal("GetSettings", callOrder[0]);
        Assert.Equal("UpdateSettings", callOrder[1]);
        Assert.Equal("PublishEvent", callOrder[2]);
        Assert.Equal("CreateLog", callOrder[3]);
    }
}