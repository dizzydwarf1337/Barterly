using Application.Commands.Admins.Posts.UpdatePostSettings;
using Application.Core.MediatR.Requests;
using Domain.Enums.Posts;
using Domain.Enums.Users;
using Domain.Interfaces.Commands.Post;
using Moq;

namespace BarterlyUnitTests.Commands.Admins.Posts;

public class UpdatePostSettings
{
    private readonly Mock<IPostSettingsCommandRepository> _postSettingsCommandRepositoryMock;
    private readonly UpdatePostSettingsCommandHandler _handler;

    public UpdatePostSettings()
    {
        _postSettingsCommandRepositoryMock = new Mock<IPostSettingsCommandRepository>();
        _handler = new UpdatePostSettingsCommandHandler(
            _postSettingsCommandRepositoryMock.Object
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
    public async Task Handle_WhenValidCommand_UpdatesPostSettings()
    {
        // Arrange
        var settingsId = Guid.NewGuid();
        var command = new UpdatePostSettingsCommand
        {
            Id = settingsId,
            IsHidden = true,
            IsDeleted = false,
            PostStatusType = PostStatusType.Published,
            RejectionMessage = null,
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
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

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_CallsUpdatePostSettingsWithCorrectParameters()
    {
        // Arrange
        var settingsId = Guid.NewGuid();
        var command = new UpdatePostSettingsCommand
        {
            Id = settingsId,
            IsHidden = true,
            IsDeleted = false,
            PostStatusType = PostStatusType.Rejected,
            RejectionMessage = "Invalid content",
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
        };

        _postSettingsCommandRepositoryMock
            .Setup(x => x.UpdatePostSettings(
                settingsId,
                It.IsAny<CancellationToken>(),
                true,
                false,
                PostStatusType.Rejected,
                "Invalid content"))
            .Returns(() => Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _postSettingsCommandRepositoryMock.Verify(
            x => x.UpdatePostSettings(
                settingsId,
                It.IsAny<CancellationToken>(),
                true,
                false,
                PostStatusType.Rejected,
                "Invalid content"),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithIsHiddenTrue_UpdatesCorrectly()
    {
        // Arrange
        var settingsId = Guid.NewGuid();
        var command = new UpdatePostSettingsCommand
        {
            Id = settingsId,
            IsHidden = true,
            IsDeleted = false,
            PostStatusType = PostStatusType.Published,
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
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

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _postSettingsCommandRepositoryMock.Verify(
            x => x.UpdatePostSettings(
                settingsId,
                It.IsAny<CancellationToken>(),
                true,
                It.IsAny<bool>(),
                It.IsAny<PostStatusType>(),
                It.IsAny<string?>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithIsDeletedTrue_UpdatesCorrectly()
    {
        // Arrange
        var settingsId = Guid.NewGuid();
        var command = new UpdatePostSettingsCommand
        {
            Id = settingsId,
            IsHidden = false,
            IsDeleted = true,
            PostStatusType = PostStatusType.Deleted,
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
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

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _postSettingsCommandRepositoryMock.Verify(
            x => x.UpdatePostSettings(
                settingsId,
                It.IsAny<CancellationToken>(),
                It.IsAny<bool>(),
                true,
                It.IsAny<PostStatusType>(),
                It.IsAny<string?>()),
            Times.Once);
    }

    [Theory]
    [InlineData(PostStatusType.Published)]
    [InlineData(PostStatusType.Rejected)]
    [InlineData(PostStatusType.Deleted)]
    [InlineData(PostStatusType.UnderReview)]
    [InlineData(PostStatusType.ReSubmitted)]
    public async Task Handle_WithDifferentPostStatusTypes_UpdatesCorrectly(PostStatusType statusType)
    {
        // Arrange
        var settingsId = Guid.NewGuid();
        var command = new UpdatePostSettingsCommand
        {
            Id = settingsId,
            IsHidden = false,
            IsDeleted = false,
            PostStatusType = statusType,
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
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

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _postSettingsCommandRepositoryMock.Verify(
            x => x.UpdatePostSettings(
                settingsId,
                It.IsAny<CancellationToken>(),
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                statusType,
                It.IsAny<string?>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithRejectionMessage_PassesMessageCorrectly()
    {
        // Arrange
        var settingsId = Guid.NewGuid();
        var rejectionMessage = "Content violates policy";
        var command = new UpdatePostSettingsCommand
        {
            Id = settingsId,
            IsHidden = true,
            IsDeleted = false,
            PostStatusType = PostStatusType.Rejected,
            RejectionMessage = rejectionMessage,
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
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

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _postSettingsCommandRepositoryMock.Verify(
            x => x.UpdatePostSettings(
                settingsId,
                It.IsAny<CancellationToken>(),
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<PostStatusType>(),
                rejectionMessage),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithNullRejectionMessage_PassesNullCorrectly()
    {
        // Arrange
        var settingsId = Guid.NewGuid();
        var command = new UpdatePostSettingsCommand
        {
            Id = settingsId,
            IsHidden = false,
            IsDeleted = false,
            PostStatusType = PostStatusType.Published,
            RejectionMessage = null,
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
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

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _postSettingsCommandRepositoryMock.Verify(
            x => x.UpdatePostSettings(
                settingsId,
                It.IsAny<CancellationToken>(),
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<PostStatusType>(),
                null),
            Times.Once);
    }

    [Fact]
    public async Task Handle_PassesCancellationTokenToRepository()
    {
        // Arrange
        var settingsId = Guid.NewGuid();
        var command = new UpdatePostSettingsCommand
        {
            Id = settingsId,
            IsHidden = false,
            IsDeleted = false,
            PostStatusType = PostStatusType.Published,
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
        };

        var cancellationToken = new CancellationToken();

        _postSettingsCommandRepositoryMock
            .Setup(x => x.UpdatePostSettings(
                settingsId,
                cancellationToken,
                false,
                false,
                PostStatusType.Published,
                null))
            .Returns(() => Task.CompletedTask);

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        _postSettingsCommandRepositoryMock.Verify(
            x => x.UpdatePostSettings(
                settingsId,
                cancellationToken,
                false,
                false,
                PostStatusType.Published,
                null),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ReturnsSuccessResponse()
    {
        // Arrange
        var settingsId = Guid.NewGuid();
        var command = new UpdatePostSettingsCommand
        {
            Id = settingsId,
            IsHidden = false,
            IsDeleted = false,
            PostStatusType = PostStatusType.Published,
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
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

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_WithDifferentSettingsIds_UpdatesCorrectSettings()
    {
        // Arrange
        var settingsId1 = Guid.NewGuid();
        var settingsId2 = Guid.NewGuid();

        var command = new UpdatePostSettingsCommand
        {
            Id = settingsId1,
            IsHidden = false,
            IsDeleted = false,
            PostStatusType = PostStatusType.Published,
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
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

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _postSettingsCommandRepositoryMock.Verify(
            x => x.UpdatePostSettings(settingsId1, It.IsAny<CancellationToken>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<PostStatusType>(), It.IsAny<string?>()),
            Times.Once);
        _postSettingsCommandRepositoryMock.Verify(
            x => x.UpdatePostSettings(settingsId2, It.IsAny<CancellationToken>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<PostStatusType>(), It.IsAny<string?>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WithAllParametersCombination_UpdatesCorrectly()
    {
        // Arrange
        var settingsId = Guid.NewGuid();
        var command = new UpdatePostSettingsCommand
        {
            Id = settingsId,
            IsHidden = true,
            IsDeleted = true,
            PostStatusType = PostStatusType.Rejected,
            RejectionMessage = "Multiple violations detected",
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
        };

        _postSettingsCommandRepositoryMock
            .Setup(x => x.UpdatePostSettings(
                settingsId,
                It.IsAny<CancellationToken>(),
                true,
                true,
                PostStatusType.Rejected,
                "Multiple violations detected"))
            .Returns(() => Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _postSettingsCommandRepositoryMock.Verify(
            x => x.UpdatePostSettings(
                settingsId,
                It.IsAny<CancellationToken>(),
                true,
                true,
                PostStatusType.Rejected,
                "Multiple violations detected"),
            Times.Once);
    }
}