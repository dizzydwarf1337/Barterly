using Moq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Application.Services;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Commands.General;
using Domain.Interfaces.Queries.General;
using Persistence.Repositories.Commands.General;
using Persistence.Repositories.Queries.General;
using Xunit;
using Persistence.Database;
using AutoMapper;
using Application.Interfaces;
using Application.DTOs;

namespace BarterlyUnitTests.ServiceTests.General
{
    public class LogServiceTests
    {
        private readonly Mock<ILogCommandRepository> _logCommandRepoMock;
        private readonly Mock<ILogQueryRepository> _logQueryRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly LogService _logService;

        public LogServiceTests()
        {
            _logCommandRepoMock = new Mock<ILogCommandRepository>();
            _logQueryRepoMock = new Mock<ILogQueryRepository>();
            _mapperMock = new Mock<IMapper>();

            _logService = new LogService(
                _logCommandRepoMock.Object,
                _logQueryRepoMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public async Task CreateLogAsync_ShouldCallCreateLogAsync()
        {
            // Arrange
            var logDto = new LogDto { Message = "Test log", LogType = LogType.None };
            var logEntity = new Log { Message = "Test log", LogType = LogType.None };

            _mapperMock.Setup(m => m.Map<Log>(logDto)).Returns(logEntity);
            _logCommandRepoMock.Setup(r => r.CreateLogAsync(It.IsAny<Log>())).Returns(Task.CompletedTask);

            // Act
            await _logService.CreateLogAsync(logDto);

            // Assert
            _logCommandRepoMock.Verify(r => r.CreateLogAsync(It.Is<Log>(l => l.Message == "Test log")), Times.Once);
        }

        [Fact]
        public async Task CreateLogAsync_WithParams_ShouldCallCreateLogAsync()
        {
            // Arrange
            var message = "Log message";
            var logType = LogType.Warning;

            _logCommandRepoMock.Setup(r => r.CreateLogAsync(It.IsAny<Log>())).Returns(Task.CompletedTask);

            // Act
            await _logService.CreateLogAsync(message, logType);

            // Assert
            _logCommandRepoMock.Verify(r => r.CreateLogAsync(It.Is<Log>(l => l.Message == message && l.LogType == logType)), Times.Once);
        }

        [Fact]
        public async Task DeleteLogAsync_ShouldCallDeleteLogAsync()
        {
            // Arrange
            var logId = Guid.NewGuid();

            _logCommandRepoMock.Setup(r => r.DeleteLogAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask);

            // Act
            await _logService.DeleteLogAsync(logId);

            // Assert
            _logCommandRepoMock.Verify(r => r.DeleteLogAsync(logId), Times.Once);
        }

        [Fact]
        public async Task GetLogsPaginatedAsync_ShouldReturnMappedLogs()
        {
            // Arrange
            var logs = new List<Log>
        {
            new Log { Message = "Log1", LogType = LogType.None },
            new Log { Message = "Log2", LogType = LogType.Warning }
        };

            var logDtos = new List<LogDto>
        {
            new LogDto { Message = "Log1", LogType = LogType.None },
            new LogDto { Message = "Log2", LogType = LogType.Warning }
        };

            _logQueryRepoMock.Setup(r => r.GetLogsPaginatedAsync(2, 1)).ReturnsAsync(logs);
            _mapperMock.Setup(m => m.Map<ICollection<LogDto>>(logs)).Returns(logDtos);

            // Act
            var result = await _logService.GetLogsPaginatedAsync(2, 1);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Log1", result.First().Message);
            _logQueryRepoMock.Verify(r => r.GetLogsPaginatedAsync(2, 1), Times.Once);
        }

        [Fact]
        public async Task GetPostLogsAsync_ShouldReturnMappedLogs()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var logs = new List<Log> { new Log { PostId = postId, Message = "Post log" } };
            var logDtos = new List<LogDto> { new LogDto { Message = "Post log" } };

            _logQueryRepoMock.Setup(r => r.GetLogsByPostIdAsync(postId)).ReturnsAsync(logs);
            _mapperMock.Setup(m => m.Map<ICollection<LogDto>>(logs)).Returns(logDtos);

            // Act
            var result = await _logService.GetPostLogsAsync(postId);

            // Assert
            Assert.Single(result);
            Assert.Equal("Post log", result.First().Message);
            _logQueryRepoMock.Verify(r => r.GetLogsByPostIdAsync(postId), Times.Once);
        }

        [Fact]
        public async Task GetUserLogsAsync_ShouldReturnMappedLogs()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var logs = new List<Log> { new Log { UserId = userId, Message = "User log" } };
            var logDtos = new List<LogDto> { new LogDto { Message = "User log" } };

            _logQueryRepoMock.Setup(r => r.GetLogsByUserIdAsync(userId)).ReturnsAsync(logs);
            _mapperMock.Setup(m => m.Map<ICollection<LogDto>>(logs)).Returns(logDtos);

            // Act
            var result = await _logService.GetUserLogsAsync(userId);

            // Assert
            Assert.Single(result);
            Assert.Equal("User log", result.First().Message);
            _logQueryRepoMock.Verify(r => r.GetLogsByUserIdAsync(userId), Times.Once);
        }
        [Fact]
        public async Task DeletePostLogsAsync_ShouldCallDeletePostLogsWithProvidedPostId()
        {
            // Arrange
            var postId = Guid.NewGuid();

            _logCommandRepoMock.Setup(r => r.DeletePostsLogs(It.IsAny<Guid>())).Returns(Task.CompletedTask);

            // Act
            await _logService.DeletePostLogsAsync(postId);

            // Assert
            _logCommandRepoMock.Verify(r => r.DeletePostsLogs(postId), Times.Once);
        }
        [Fact]
        public async Task DeleteUserLogsAsync_ShouldCallDeleteUsersLogsWithProvidedUserId()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _logCommandRepoMock.Setup(r => r.DeleteUsersLogs(It.IsAny<Guid>())).Returns(Task.CompletedTask);

            // Act
            await _logService.DeleteUserLogsAsync(userId);

            // Assert 
            _logCommandRepoMock.Verify(r => r.DeleteUsersLogs(userId), Times.Once);
        }
    }
}
