using Application.Commands.Admins.Opinions.DeleteOpinion;
using Application.Core.MediatR.Requests;
using Domain.Enums.Users;
using Domain.Interfaces.Commands.Post;
using Domain.Interfaces.Commands.User;
using Moq;

namespace BarterlyUnitTests.Commands.Admins.Opinions;

public class DeleteOpinion
{
    private readonly Mock<IPostOpinionCommandRepository> _postOpinionCommandRepositoryMock;
    private readonly Mock<IUserOpinionCommandRepository> _userOpinionCommandRepositoryMock;
    private readonly DeleteOpinionCommandHandler _handler;

    public DeleteOpinion()
    {
        _postOpinionCommandRepositoryMock = new Mock<IPostOpinionCommandRepository>();
        _userOpinionCommandRepositoryMock = new Mock<IUserOpinionCommandRepository>();
        _handler = new DeleteOpinionCommandHandler(
            _postOpinionCommandRepositoryMock.Object,
            _userOpinionCommandRepositoryMock.Object
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
    public async Task Handle_WhenPostOpinionExists_DeletesPostOpinion()
    {
        // Arrange
        var opinionId = Guid.NewGuid();
        var command = new DeleteOpinionCommand
        {
            OpinionId = opinionId,
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
        };

        _postOpinionCommandRepositoryMock
            .Setup(x => x.DeletePostOpinionAsync(opinionId, It.IsAny<CancellationToken>()))
            .Returns(() => Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _postOpinionCommandRepositoryMock.Verify(
            x => x.DeletePostOpinionAsync(opinionId, It.IsAny<CancellationToken>()),
            Times.Once);
        _userOpinionCommandRepositoryMock.Verify(
            x => x.DeleteUserOpinionAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenPostOpinionDeleteFails_DeletesUserOpinion()
    {
        // Arrange
        var opinionId = Guid.NewGuid();
        var command = new DeleteOpinionCommand
        {
            OpinionId = opinionId,
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
        };

        _postOpinionCommandRepositoryMock
            .Setup(x => x.DeletePostOpinionAsync(opinionId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Post opinion not found"));

        _userOpinionCommandRepositoryMock
            .Setup(x => x.DeleteUserOpinionAsync(opinionId, It.IsAny<CancellationToken>()))
            .Returns(() => Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _postOpinionCommandRepositoryMock.Verify(
            x => x.DeletePostOpinionAsync(opinionId, It.IsAny<CancellationToken>()),
            Times.Once);
        _userOpinionCommandRepositoryMock.Verify(
            x => x.DeleteUserOpinionAsync(opinionId, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_CallsPostOpinionDeleteFirst()
    {
        // Arrange
        var opinionId = Guid.NewGuid();
        var command = new DeleteOpinionCommand
        {
            OpinionId = opinionId,
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
        };

        var callOrder = new List<string>();

        _postOpinionCommandRepositoryMock
            .Setup(x => x.DeletePostOpinionAsync(opinionId, It.IsAny<CancellationToken>()))
            .Callback(() => callOrder.Add("PostOpinion"))
            .Returns(() => Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Single(callOrder);
        Assert.Equal("PostOpinion", callOrder[0]);
    }

    [Fact]
    public async Task Handle_WhenPostOpinionFailsWithAnyException_TriesUserOpinion()
    {
        // Arrange
        var opinionId = Guid.NewGuid();
        var command = new DeleteOpinionCommand
        {
            OpinionId = opinionId,
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
        };

        _postOpinionCommandRepositoryMock
            .Setup(x => x.DeletePostOpinionAsync(opinionId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Any exception"));

        _userOpinionCommandRepositoryMock
            .Setup(x => x.DeleteUserOpinionAsync(opinionId, It.IsAny<CancellationToken>()))
            .Returns(() => Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _userOpinionCommandRepositoryMock.Verify(
            x => x.DeleteUserOpinionAsync(opinionId, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_AlwaysReturnsSuccess()
    {
        // Arrange
        var opinionId = Guid.NewGuid();
        var command = new DeleteOpinionCommand
        {
            OpinionId = opinionId,
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
        };

        _postOpinionCommandRepositoryMock
            .Setup(x => x.DeletePostOpinionAsync(opinionId, It.IsAny<CancellationToken>()))
            .Returns(() => Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task Handle_PassesCancellationTokenToPostOpinionRepository()
    {
        // Arrange
        var opinionId = Guid.NewGuid();
        var command = new DeleteOpinionCommand
        {
            OpinionId = opinionId,
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
        };

        var cancellationToken = new CancellationToken();

        _postOpinionCommandRepositoryMock
            .Setup(x => x.DeletePostOpinionAsync(opinionId, cancellationToken))
            .Returns(() => Task.CompletedTask);

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        _postOpinionCommandRepositoryMock.Verify(
            x => x.DeletePostOpinionAsync(opinionId, cancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task Handle_PassesCancellationTokenToUserOpinionRepository()
    {
        // Arrange
        var opinionId = Guid.NewGuid();
        var command = new DeleteOpinionCommand
        {
            OpinionId = opinionId,
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
        };

        var cancellationToken = new CancellationToken();

        _postOpinionCommandRepositoryMock
            .Setup(x => x.DeletePostOpinionAsync(opinionId, cancellationToken))
            .ThrowsAsync(new Exception("Not found"));

        _userOpinionCommandRepositoryMock
            .Setup(x => x.DeleteUserOpinionAsync(opinionId, cancellationToken))
            .Returns(() => Task.CompletedTask);

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        _userOpinionCommandRepositoryMock.Verify(
            x => x.DeleteUserOpinionAsync(opinionId, cancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithDifferentOpinionIds_CallsRepositoriesWithCorrectId()
    {
        // Arrange
        var opinionId1 = Guid.NewGuid();
        var opinionId2 = Guid.NewGuid();

        var command = new DeleteOpinionCommand
        {
            OpinionId = opinionId1,
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
        };

        _postOpinionCommandRepositoryMock
            .Setup(x => x.DeletePostOpinionAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns(() => Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _postOpinionCommandRepositoryMock.Verify(
            x => x.DeletePostOpinionAsync(opinionId1, It.IsAny<CancellationToken>()),
            Times.Once);
        _postOpinionCommandRepositoryMock.Verify(
            x => x.DeletePostOpinionAsync(opinionId2, It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_FallbackLogic_DeletesUserOpinionWithSameId()
    {
        // Arrange
        var opinionId = Guid.NewGuid();
        var command = new DeleteOpinionCommand
        {
            OpinionId = opinionId,
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
        };

        _postOpinionCommandRepositoryMock
            .Setup(x => x.DeletePostOpinionAsync(opinionId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Not post opinion"));

        _userOpinionCommandRepositoryMock
            .Setup(x => x.DeleteUserOpinionAsync(opinionId, It.IsAny<CancellationToken>()))
            .Returns(() => Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _userOpinionCommandRepositoryMock.Verify(
            x => x.DeleteUserOpinionAsync(opinionId, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}