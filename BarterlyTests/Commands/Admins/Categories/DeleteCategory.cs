using Application.Commands.Admins.Categories.DeleteCategory;
using Application.Core.MediatR.Requests;
using Application.Interfaces;
using Domain.Enums.Common;
using Domain.Enums.Users;
using Domain.Interfaces.Commands.Post;
using Moq;

namespace BarterlyUnitTests.Commands.Admins.Categories;

public class DeleteCategory
{
    private readonly Mock<ICategoryCommandRepository> _categoryCommandRepositoryMock;
    private readonly Mock<ILogService> _logServiceMock;
    private readonly DeleteCategoryCommandHandler _handler;

    public DeleteCategory()
    {
        _categoryCommandRepositoryMock = new Mock<ICategoryCommandRepository>();
        _logServiceMock = new Mock<ILogService>();
        _handler = new DeleteCategoryCommandHandler(
            _categoryCommandRepositoryMock.Object,
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
    public async Task Handle_WhenValidCommand_DeletesCategory()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var command = new DeleteCategoryCommand
        {
            CategoryId = categoryId,
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
        };

        _categoryCommandRepositoryMock
            .Setup(x => x.DeleteCategoryAsync(categoryId, It.IsAny<CancellationToken>()))
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
        Assert.Null(result.Error);
    }

    [Fact]
    public async Task Handle_CallsDeleteCategoryAsyncWithCorrectId()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var command = new DeleteCategoryCommand
        {
            CategoryId = categoryId,
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
        };

        _categoryCommandRepositoryMock
            .Setup(x => x.DeleteCategoryAsync(categoryId, It.IsAny<CancellationToken>()))
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
        _categoryCommandRepositoryMock.Verify(
            x => x.DeleteCategoryAsync(categoryId, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_CreatesLogWithCorrectInformation()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var command = new DeleteCategoryCommand
        {
            CategoryId = categoryId,
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
        };

        _categoryCommandRepositoryMock
            .Setup(x => x.DeleteCategoryAsync(categoryId, It.IsAny<CancellationToken>()))
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
                It.Is<string>(msg => msg.Contains(categoryId.ToString()) && msg.Contains("Category deleted")),
                It.IsAny<CancellationToken>(),
                LogType.Information,
                It.IsAny<string?>(),
                It.IsAny<Guid?>(),
                It.IsAny<Guid?>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_LogIsCreatedWithInformationType()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var command = new DeleteCategoryCommand
        {
            CategoryId = categoryId,
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
        };

        _categoryCommandRepositoryMock
            .Setup(x => x.DeleteCategoryAsync(categoryId, It.IsAny<CancellationToken>()))
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
                It.IsAny<string>(),
                It.IsAny<CancellationToken>(),
                LogType.Information,
                It.IsAny<string?>(),
                It.IsAny<Guid?>(),
                It.IsAny<Guid?>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ReturnsSuccessResponse()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var command = new DeleteCategoryCommand
        {
            CategoryId = categoryId,
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
        };

        _categoryCommandRepositoryMock
            .Setup(x => x.DeleteCategoryAsync(categoryId, It.IsAny<CancellationToken>()))
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
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task Handle_PassesCancellationTokenToRepository()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var command = new DeleteCategoryCommand
        {
            CategoryId = categoryId,
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
        };

        var cancellationToken = new CancellationToken();

        _categoryCommandRepositoryMock
            .Setup(x => x.DeleteCategoryAsync(categoryId, cancellationToken))
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
        _categoryCommandRepositoryMock.Verify(
            x => x.DeleteCategoryAsync(categoryId, cancellationToken),
            Times.Once);
        _logServiceMock.Verify(
            x => x.CreateLogAsync(
                It.IsAny<string>(), 
                cancellationToken, 
                It.IsAny<LogType?>(), 
                It.IsAny<string?>(), 
                It.IsAny<Guid?>(), 
                It.IsAny<Guid?>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_DeletesCategoryBeforeCreatingLog()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var command = new DeleteCategoryCommand
        {
            CategoryId = categoryId,
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
        };

        var callOrder = new List<string>();

        _categoryCommandRepositoryMock
            .Setup(x => x.DeleteCategoryAsync(categoryId, It.IsAny<CancellationToken>()))
            .Callback(() => callOrder.Add("Delete"))
            .Returns(() => Task.CompletedTask);

        _logServiceMock
            .Setup(x => x.CreateLogAsync(
                It.IsAny<string>(), 
                It.IsAny<CancellationToken>(), 
                It.IsAny<LogType?>(), 
                It.IsAny<string?>(), 
                It.IsAny<Guid?>(), 
                It.IsAny<Guid?>()))
            .Callback(() => callOrder.Add("Log"))
            .Returns(() => Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(2, callOrder.Count);
        Assert.Equal("Delete", callOrder[0]);
        Assert.Equal("Log", callOrder[1]);
    }

    [Fact]
    public async Task Handle_WithDifferentCategoryIds_CallsRepositoryWithCorrectId()
    {
        // Arrange
        var categoryId1 = Guid.NewGuid();
        var categoryId2 = Guid.NewGuid();

        var command1 = new DeleteCategoryCommand
        {
            CategoryId = categoryId1,
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
        };

        _categoryCommandRepositoryMock
            .Setup(x => x.DeleteCategoryAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
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
        await _handler.Handle(command1, CancellationToken.None);

        // Assert
        _categoryCommandRepositoryMock.Verify(
            x => x.DeleteCategoryAsync(categoryId1, It.IsAny<CancellationToken>()),
            Times.Once);
        _categoryCommandRepositoryMock.Verify(
            x => x.DeleteCategoryAsync(categoryId2, It.IsAny<CancellationToken>()),
            Times.Never);
    }
}