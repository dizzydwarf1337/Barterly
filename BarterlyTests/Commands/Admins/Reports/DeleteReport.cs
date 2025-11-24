using Application.Commands.Admins.Reports.DeleteReport;
using Application.Core.MediatR.Requests;
using Domain.Enums.Users;
using Domain.Interfaces.Commands.Post;
using Domain.Interfaces.Commands.User;
using Moq;

namespace BarterlyUnitTests.Commands.Admins.Reports;

public class DeleteReport
{
    private readonly Mock<IReportPostCommandRepository> _reportPostCommandRepositoryMock;
    private readonly Mock<IReportUserCommandRepository> _reportUserCommandRepositoryMock;
    private readonly DeleteReportCommandHandler _handler;

    public DeleteReport()
    {
        _reportPostCommandRepositoryMock = new Mock<IReportPostCommandRepository>();
        _reportUserCommandRepositoryMock = new Mock<IReportUserCommandRepository>();
        _handler = new DeleteReportCommandHandler(
            _reportPostCommandRepositoryMock.Object,
            _reportUserCommandRepositoryMock.Object
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
    public async Task Handle_WhenPostReportExists_DeletesPostReport()
    {
        // Arrange
        var reportId = Guid.NewGuid();
        var command = new DeleteReportCommand
        {
            ReportId = reportId,
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
        };

        _reportPostCommandRepositoryMock
            .Setup(x => x.DeleteReportPostAsync(reportId, It.IsAny<CancellationToken>()))
            .Returns(() => Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _reportPostCommandRepositoryMock.Verify(
            x => x.DeleteReportPostAsync(reportId, It.IsAny<CancellationToken>()),
            Times.Once);
        _reportUserCommandRepositoryMock.Verify(
            x => x.DeleteReport(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenPostReportDeleteFails_DeletesUserReport()
    {
        // Arrange
        var reportId = Guid.NewGuid();
        var command = new DeleteReportCommand
        {
            ReportId = reportId,
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
        };

        _reportPostCommandRepositoryMock
            .Setup(x => x.DeleteReportPostAsync(reportId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Post report not found"));

        _reportUserCommandRepositoryMock
            .Setup(x => x.DeleteReport(reportId, It.IsAny<CancellationToken>()))
            .Returns(() => Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _reportPostCommandRepositoryMock.Verify(
            x => x.DeleteReportPostAsync(reportId, It.IsAny<CancellationToken>()),
            Times.Once);
        _reportUserCommandRepositoryMock.Verify(
            x => x.DeleteReport(reportId, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_CallsPostReportDeleteFirst()
    {
        // Arrange
        var reportId = Guid.NewGuid();
        var command = new DeleteReportCommand
        {
            ReportId = reportId,
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
        };

        var callOrder = new List<string>();

        _reportPostCommandRepositoryMock
            .Setup(x => x.DeleteReportPostAsync(reportId, It.IsAny<CancellationToken>()))
            .Callback(() => callOrder.Add("PostReport"))
            .Returns(() => Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Single(callOrder);
        Assert.Equal("PostReport", callOrder[0]);
    }

    [Fact]
    public async Task Handle_WhenPostReportFailsWithAnyException_TriesUserReport()
    {
        // Arrange
        var reportId = Guid.NewGuid();
        var command = new DeleteReportCommand
        {
            ReportId = reportId,
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
        };

        _reportPostCommandRepositoryMock
            .Setup(x => x.DeleteReportPostAsync(reportId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Any exception"));

        _reportUserCommandRepositoryMock
            .Setup(x => x.DeleteReport(reportId, It.IsAny<CancellationToken>()))
            .Returns(() => Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _reportUserCommandRepositoryMock.Verify(
            x => x.DeleteReport(reportId, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_AlwaysReturnsSuccess()
    {
        // Arrange
        var reportId = Guid.NewGuid();
        var command = new DeleteReportCommand
        {
            ReportId = reportId,
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
        };

        _reportPostCommandRepositoryMock
            .Setup(x => x.DeleteReportPostAsync(reportId, It.IsAny<CancellationToken>()))
            .Returns(() => Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_PassesCancellationTokenToPostReportRepository()
    {
        // Arrange
        var reportId = Guid.NewGuid();
        var command = new DeleteReportCommand
        {
            ReportId = reportId,
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
        };

        var cancellationToken = new CancellationToken();

        _reportPostCommandRepositoryMock
            .Setup(x => x.DeleteReportPostAsync(reportId, cancellationToken))
            .Returns(() => Task.CompletedTask);

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        _reportPostCommandRepositoryMock.Verify(
            x => x.DeleteReportPostAsync(reportId, cancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task Handle_PassesCancellationTokenToUserReportRepository()
    {
        // Arrange
        var reportId = Guid.NewGuid();
        var command = new DeleteReportCommand
        {
            ReportId = reportId,
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
        };

        var cancellationToken = new CancellationToken();

        _reportPostCommandRepositoryMock
            .Setup(x => x.DeleteReportPostAsync(reportId, cancellationToken))
            .ThrowsAsync(new Exception("Not found"));

        _reportUserCommandRepositoryMock
            .Setup(x => x.DeleteReport(reportId, cancellationToken))
            .Returns(() => Task.CompletedTask);

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        _reportUserCommandRepositoryMock.Verify(
            x => x.DeleteReport(reportId, cancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithDifferentReportIds_CallsRepositoriesWithCorrectId()
    {
        // Arrange
        var reportId1 = Guid.NewGuid();
        var reportId2 = Guid.NewGuid();

        var command = new DeleteReportCommand
        {
            ReportId = reportId1,
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
        };

        _reportPostCommandRepositoryMock
            .Setup(x => x.DeleteReportPostAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns(() => Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _reportPostCommandRepositoryMock.Verify(
            x => x.DeleteReportPostAsync(reportId1, It.IsAny<CancellationToken>()),
            Times.Once);
        _reportPostCommandRepositoryMock.Verify(
            x => x.DeleteReportPostAsync(reportId2, It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_FallbackLogic_DeletesUserReportWithSameId()
    {
        // Arrange
        var reportId = Guid.NewGuid();
        var command = new DeleteReportCommand
        {
            ReportId = reportId,
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
        };

        _reportPostCommandRepositoryMock
            .Setup(x => x.DeleteReportPostAsync(reportId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Not post report"));

        _reportUserCommandRepositoryMock
            .Setup(x => x.DeleteReport(reportId, It.IsAny<CancellationToken>()))
            .Returns(() => Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _reportUserCommandRepositoryMock.Verify(
            x => x.DeleteReport(reportId, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenBothRepositoriesSucceed_ReturnsSuccess()
    {
        // Arrange
        var reportId = Guid.NewGuid();
        var command = new DeleteReportCommand
        {
            ReportId = reportId,
            AuthorizeData = CreateAuthorizeData(Guid.NewGuid())
        };

        _reportPostCommandRepositoryMock
            .Setup(x => x.DeleteReportPostAsync(reportId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Post not found"));

        _reportUserCommandRepositoryMock
            .Setup(x => x.DeleteReport(reportId, It.IsAny<CancellationToken>()))
            .Returns(() => Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
    }
}