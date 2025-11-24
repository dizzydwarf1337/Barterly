using Application.Queries.Admins.Reports.GetReportsFiltredPaginated;
using AutoMapper;
using Domain.Entities.Posts;
using Domain.Entities.Users;
using Domain.Enums.Common;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Queries.Post;
using Domain.Interfaces.Queries.User;
using Moq;

namespace BarterlyUnitTests.Queries.Admin.Reports;

public class GetReportFiltredPaginatedQueryHandlerTests
{
    private readonly Mock<IReportPostQueryRepository> _postReportRepositoryMock;
    private readonly Mock<IReportUserQueryRepository> _userReportRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetReportFiltredPaginatedQueryHandler _handler;

    public GetReportFiltredPaginatedQueryHandlerTests()
    {
        _postReportRepositoryMock = new Mock<IReportPostQueryRepository>();
        _userReportRepositoryMock = new Mock<IReportUserQueryRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetReportFiltredPaginatedQueryHandler(
            _postReportRepositoryMock.Object,
            _userReportRepositoryMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_WhenSubjectIdIsNull_ThrowsEntityNotFoundException()
    {
        // Arrange
        var query = new GetReportFiltredPaginatedQuery
        {
            SubjectId = null,
            Page = 1,
            PageSize = 10
        };

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(
            async () => await _handler.Handle(query, CancellationToken.None)
        );
    }

    [Fact]
    public async Task Handle_WhenPostReportsExist_ReturnsPostReports()
    {
        // Arrange
        var subjectId = Guid.NewGuid();
        var authorId = Guid.NewGuid();
        
        var postReports = new List<ReportPost>
        {
            new ReportPost
            {
                Id = Guid.NewGuid(),
                AuthorId = authorId,
                ReportedPostId = subjectId,
                Message = "Test report 1",
                CreatedAt = DateTime.UtcNow,
                Status = ReportStatusType.Submitted
            },
            new ReportPost
            {
                Id = Guid.NewGuid(),
                AuthorId = authorId,
                ReportedPostId = subjectId,
                Message = "Test report 2",
                CreatedAt = DateTime.UtcNow,
                Status = ReportStatusType.Submitted
            }
        };

        _postReportRepositoryMock
            .Setup(x => x.GetReportPostsFiltredPaginated(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Guid?>(),
                It.IsAny<Guid?>(),
                It.IsAny<ReportStatusType?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(postReports);

        var query = new GetReportFiltredPaginatedQuery
        {
            SubjectId = subjectId,
            Page = 1,
            PageSize = 10
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(2, result.Value.Reports.Count());
        Assert.All(result.Value.Reports, report => Assert.Equal(subjectId, report.SubjectId));

        _postReportRepositoryMock.Verify(
            x => x.GetReportPostsFiltredPaginated(10, 1, null, subjectId, null, It.IsAny<CancellationToken>()),
            Times.Once);
        
        _userReportRepositoryMock.Verify(
            x => x.GetReportUserFiltredPaginated(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Guid?>(),
                It.IsAny<Guid?>(),
                It.IsAny<ReportStatusType?>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenPostReportsEmpty_ReturnsUserReports()
    {
        // Arrange
        var subjectId = Guid.NewGuid();
        var authorId = Guid.NewGuid();
        
        var emptyPostReports = new List<ReportPost>();
        
        var userReports = new List<ReportUser>
        {
            new ReportUser
            {
                Id = Guid.NewGuid(),
                AuthorId = authorId,
                ReportedUserId = subjectId,
                Message = "User report 1",
                CreatedAt = DateTime.UtcNow,
                Status = ReportStatusType.Submitted
            },
            new ReportUser
            {
                Id = Guid.NewGuid(),
                AuthorId = authorId,
                ReportedUserId = subjectId,
                Message = "User report 2",
                CreatedAt = DateTime.UtcNow,
                Status = ReportStatusType.Submitted
            }
        };

        _postReportRepositoryMock
            .Setup(x => x.GetReportPostsFiltredPaginated(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Guid?>(),
                It.IsAny<Guid?>(),
                It.IsAny<ReportStatusType?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(emptyPostReports);

        _userReportRepositoryMock
            .Setup(x => x.GetReportUserFiltredPaginated(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Guid?>(),
                It.IsAny<Guid?>(),
                It.IsAny<ReportStatusType?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(userReports);

        var query = new GetReportFiltredPaginatedQuery
        {
            SubjectId = subjectId,
            Page = 1,
            PageSize = 10
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(2, result.Value.Reports.Count());
        Assert.All(result.Value.Reports, report => Assert.Equal(subjectId, report.SubjectId));

        _postReportRepositoryMock.Verify(
            x => x.GetReportPostsFiltredPaginated(10, 1, null, subjectId, null, It.IsAny<CancellationToken>()),
            Times.Once);
        
        _userReportRepositoryMock.Verify(
            x => x.GetReportUserFiltredPaginated(10, 1, null, subjectId, null, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenAuthorIdProvided_FiltersReportsByAuthor()
    {
        // Arrange
        var subjectId = Guid.NewGuid();
        var authorId = Guid.NewGuid();
        
        var postReports = new List<ReportPost>
        {
            new ReportPost
            {
                Id = Guid.NewGuid(),
                AuthorId = authorId,
                ReportedPostId = subjectId,
                Message = "Test report",
                CreatedAt = DateTime.UtcNow,
                Status = ReportStatusType.Submitted
            }
        };

        _postReportRepositoryMock
            .Setup(x => x.GetReportPostsFiltredPaginated(
                It.IsAny<int>(),
                It.IsAny<int>(),
                authorId,
                It.IsAny<Guid?>(),
                It.IsAny<ReportStatusType?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(postReports);

        var query = new GetReportFiltredPaginatedQuery
        {
            SubjectId = subjectId,
            AuthorId = authorId,
            Page = 1,
            PageSize = 10
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Value.Reports);
        Assert.Equal(authorId, result.Value.Reports.First().AuthorId);

        _postReportRepositoryMock.Verify(
            x => x.GetReportPostsFiltredPaginated(10, 1, authorId, subjectId, null, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenStatusProvided_FiltersReportsByStatus()
    {
        // Arrange
        var subjectId = Guid.NewGuid();
        var status = ReportStatusType.Approved;
        
        var postReports = new List<ReportPost>
        {
            new ReportPost
            {
                Id = Guid.NewGuid(),
                AuthorId = Guid.NewGuid(),
                ReportedPostId = subjectId,
                Message = "Test report",
                CreatedAt = DateTime.UtcNow,
                Status = status
            }
        };

        _postReportRepositoryMock
            .Setup(x => x.GetReportPostsFiltredPaginated(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Guid?>(),
                It.IsAny<Guid?>(),
                status,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(postReports);

        var query = new GetReportFiltredPaginatedQuery
        {
            SubjectId = subjectId,
            Status = status,
            Page = 1,
            PageSize = 10
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Value.Reports);

        _postReportRepositoryMock.Verify(
            x => x.GetReportPostsFiltredPaginated(10, 1, null, subjectId, status, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenPaginationProvided_UsesCorrectPageAndPageSize()
    {
        // Arrange
        var subjectId = Guid.NewGuid();
        var page = 2;
        var pageSize = 20;
        
        var postReports = new List<ReportPost>
        {
            new ReportPost
            {
                Id = Guid.NewGuid(),
                AuthorId = Guid.NewGuid(),
                ReportedPostId = subjectId,
                Message = "Test report",
                CreatedAt = DateTime.UtcNow,
                Status = ReportStatusType.Submitted
            }
        };

        _postReportRepositoryMock
            .Setup(x => x.GetReportPostsFiltredPaginated(
                pageSize,
                page,
                It.IsAny<Guid?>(),
                It.IsAny<Guid?>(),
                It.IsAny<ReportStatusType?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(postReports);

        var query = new GetReportFiltredPaginatedQuery
        {
            SubjectId = subjectId,
            Page = page,
            PageSize = pageSize
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);

        _postReportRepositoryMock.Verify(
            x => x.GetReportPostsFiltredPaginated(pageSize, page, null, subjectId, null, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenAllFiltersProvided_UsesAllFilters()
    {
        // Arrange
        var subjectId = Guid.NewGuid();
        var authorId = Guid.NewGuid();
        var status = ReportStatusType.Approved;
        var page = 3;
        var pageSize = 15;
        
        var postReports = new List<ReportPost>
        {
            new ReportPost
            {
                Id = Guid.NewGuid(),
                AuthorId = authorId,
                ReportedPostId = subjectId,
                Message = "Test report",
                CreatedAt = DateTime.UtcNow,
                Status = status
            }
        };

        _postReportRepositoryMock
            .Setup(x => x.GetReportPostsFiltredPaginated(
                pageSize,
                page,
                authorId,
                subjectId,
                status,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(postReports);

        var query = new GetReportFiltredPaginatedQuery
        {
            SubjectId = subjectId,
            AuthorId = authorId,
            Status = status,
            Page = page,
            PageSize = pageSize
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Value.Reports);

        _postReportRepositoryMock.Verify(
            x => x.GetReportPostsFiltredPaginated(pageSize, page, authorId, subjectId, status, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenNoReportsFound_ReturnsEmptyCollection()
    {
        // Arrange
        var subjectId = Guid.NewGuid();
        
        var emptyPostReports = new List<ReportPost>();
        var emptyUserReports = new List<ReportUser>();

        _postReportRepositoryMock
            .Setup(x => x.GetReportPostsFiltredPaginated(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Guid?>(),
                It.IsAny<Guid?>(),
                It.IsAny<ReportStatusType?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(emptyPostReports);

        _userReportRepositoryMock
            .Setup(x => x.GetReportUserFiltredPaginated(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Guid?>(),
                It.IsAny<Guid?>(),
                It.IsAny<ReportStatusType?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(emptyUserReports);

        var query = new GetReportFiltredPaginatedQuery
        {
            SubjectId = subjectId,
            Page = 1,
            PageSize = 10
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Empty(result.Value.Reports);
    }

    [Fact]
    public async Task Handle_MapsPostReportFieldsCorrectly()
    {
        // Arrange
        var reportId = Guid.NewGuid();
        var authorId = Guid.NewGuid();
        var subjectId = Guid.NewGuid();
        var message = "Inappropriate content";
        var createdAt = DateTime.UtcNow;
        
        var postReports = new List<ReportPost>
        {
            new ReportPost
            {
                Id = reportId,
                AuthorId = authorId,
                ReportedPostId = subjectId,
                Message = message,
                CreatedAt = createdAt,
                Status = ReportStatusType.Submitted
            }
        };

        _postReportRepositoryMock
            .Setup(x => x.GetReportPostsFiltredPaginated(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Guid?>(),
                It.IsAny<Guid?>(),
                It.IsAny<ReportStatusType?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(postReports);

        var query = new GetReportFiltredPaginatedQuery
        {
            SubjectId = subjectId,
            Page = 1,
            PageSize = 10
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        var report = result.Value.Reports.First();
        Assert.Equal(reportId, report.Id);
        Assert.Equal(authorId, report.AuthorId);
        Assert.Equal(subjectId, report.SubjectId);
        Assert.Equal(message, report.Message);
        Assert.Equal(createdAt, report.CreatedAt);
    }

    [Fact]
    public async Task Handle_MapsUserReportFieldsCorrectly()
    {
        // Arrange
        var reportId = Guid.NewGuid();
        var authorId = Guid.NewGuid();
        var subjectId = Guid.NewGuid();
        var message = "Spam account";
        var createdAt = DateTime.UtcNow;
        
        var emptyPostReports = new List<ReportPost>();
        var userReports = new List<ReportUser>
        {
            new ReportUser
            {
                Id = reportId,
                AuthorId = authorId,
                ReportedUserId = subjectId,
                Message = message,
                CreatedAt = createdAt,
                Status = ReportStatusType.Submitted
            }
        };

        _postReportRepositoryMock
            .Setup(x => x.GetReportPostsFiltredPaginated(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Guid?>(),
                It.IsAny<Guid?>(),
                It.IsAny<ReportStatusType?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(emptyPostReports);

        _userReportRepositoryMock
            .Setup(x => x.GetReportUserFiltredPaginated(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Guid?>(),
                It.IsAny<Guid?>(),
                It.IsAny<ReportStatusType?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(userReports);

        var query = new GetReportFiltredPaginatedQuery
        {
            SubjectId = subjectId,
            Page = 1,
            PageSize = 10
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        var report = result.Value.Reports.First();
        Assert.Equal(reportId, report.Id);
        Assert.Equal(authorId, report.AuthorId);
        Assert.Equal(subjectId, report.SubjectId);
        Assert.Equal(message, report.Message);
        Assert.Equal(createdAt, report.CreatedAt);
    }
}