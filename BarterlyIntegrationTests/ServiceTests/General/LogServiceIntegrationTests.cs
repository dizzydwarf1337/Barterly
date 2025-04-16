using Application.DTOs;
using Application.Interfaces;
using BarterlyIntegrationTests.Core;
using Domain.Entities.Common;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace BarterlyIntegrationTests.ServiceTests.General
{
    public class LogServiceIntegrationTests : IClassFixture<DatabaseFixture>
    {
        private readonly BarterlyDbContext _database;
        private readonly ILogService _logService;

        public LogServiceIntegrationTests(DatabaseFixture fixture)
        {
            _database = fixture.dbContext;
            _logService = fixture.logService;
        }
        [Fact]
        public async Task CreatLogAsyncWithLogDtoThenGetLog_ShouldReturnLog()
        {
            // Arrange
            var logDto = new LogDto
            {
                Id = Guid.NewGuid(),
                Message = "Test Log",
                CreatedAt = DateTime.UtcNow,
                LogType = LogType.None,
                UserId = Guid.NewGuid(),
                PostId = Guid.NewGuid()
            };

            // Act
            await _logService.CreateLogAsync(logDto);
            var savedLog = await _database.Logs.FindAsync(logDto.Id);
            // Assert
            Assert.NotNull(savedLog);
            Assert.Equal(logDto.Message, savedLog.Message);
            Assert.Equal(logDto.LogType, savedLog.LogType);
            Assert.Equal(logDto.CreatedAt, savedLog.CreatedAt);
        }
        [Fact]
        public async Task CreateLogWithProvidedData_ShouldCreateLog()
        {
            // Arrange
            var message = "test message";
            var logType = LogType.Warning;
            var stackTrace = "stack trace";
            Guid postId = Guid.Empty;
            var userId = Guid.NewGuid();
            // Act 

            await _logService.CreateLogAsync(message, logType, stackTrace, postId, userId);
            var savedLog = await _database.Logs.FirstOrDefaultAsync(x => String.Equals(x.Message.ToLower(), message.ToLower()));

            // Assert

            Assert.Equal(message, savedLog!.Message);
            Assert.Equal(logType, savedLog.LogType);
            Assert.Equal(stackTrace, savedLog.StackTrace);
            Assert.Equal(postId, savedLog.PostId);
            Assert.Equal(userId, savedLog.UserId);
        }

        [Fact]
        public async Task DeleteLogAsyncWithCreatedLog_ShouldDeleteLog()
        {
            // Arrange 
            LogDto log = new LogDto
            {
                Id = Guid.NewGuid(),
                Message = "delete",
                LogType = LogType.None,
            };
            // Act
            await _logService.CreateLogAsync(log);
            await _logService.DeleteLogAsync(log.Id);
            Log? result = await _database.Logs.FirstOrDefaultAsync(x => x.Id == log.Id);
            // Assert
            Assert.Null(result);
        }
        [Fact]
        public async Task DeletePostLogsAsync_ShouldDeletePostLogs()
        {
            //Arrange
            var postId = Guid.NewGuid();
            var log1 = new LogDto
            {
                Id = Guid.NewGuid(),
                Message = "log1",
                LogType = LogType.None,
                PostId = postId,
            };
            var log2 = new LogDto
            {
                Id = Guid.NewGuid(),
                Message = "log2",
                LogType = LogType.None,
                PostId = postId,
            };

            //Act 

            await _logService.CreateLogAsync(log1);
            await _logService.CreateLogAsync(log2);
            await _logService.DeletePostLogsAsync(postId);
            var logs = await _database.Logs.Where(x => x.PostId == postId).ToListAsync();

            //Assert
            Assert.Empty(logs);
            Assert.False(logs.Any());

        }
        [Fact]
        public async Task DeleteUserLogsAsync_ShouldDeleteUserLogs()
        {
            //Arrange

            var userId = Guid.NewGuid();
            var log1 = new LogDto
            {
                Id = Guid.NewGuid(),
                Message = "log1",
                LogType = LogType.None,
                UserId = userId,
            };
            var log2 = new LogDto
            {
                Id = Guid.NewGuid(),
                Message = "log2",
                LogType = LogType.None,
                UserId = userId,
            };

            //Act 

            await _logService.CreateLogAsync(log1);
            await _logService.CreateLogAsync(log2);
            await _logService.DeleteUserLogsAsync(userId);
            var logs = await _database.Logs.Where(x => x.UserId == userId).ToListAsync();

            //Assert

            Assert.Empty(logs);


        }
        [Fact]
        public async Task GetLogsPaginatedTwoLogsPerPage_ShouldReturnTwoDiffPages()
        {
            // Arrange
            var log1 = new LogDto
            {
                Id = Guid.NewGuid(),
                Message = "log1",
                LogType = LogType.None,
                CreatedAt = DateTime.UtcNow,
            };
            var log2 = new LogDto
            {
                Id = Guid.NewGuid(),
                Message = "log2",
                LogType = LogType.Error,
                CreatedAt = DateTime.UtcNow,
            };
            var log3 = new LogDto
            {
                Id = Guid.NewGuid(),
                Message = "log3",
                LogType = LogType.Information,
                CreatedAt = DateTime.UtcNow,
            };
            var log4 = new LogDto
            {
                Id = Guid.NewGuid(),
                Message = "log4",
                LogType = LogType.Warning,
                CreatedAt = DateTime.UtcNow,

            };

            // Act

            await _logService.CreateLogAsync(log1);
            await _logService.CreateLogAsync(log2);
            await _logService.CreateLogAsync(log3);
            await _logService.CreateLogAsync(log4);

            var logsPage1 = (await _logService.GetLogsPaginatedAsync(2, 1));
            var logsPage2 = (await _logService.GetLogsPaginatedAsync(2, 2));
            var logsTest1 = await _database.Logs.ToListAsync();
            // Assert
            Assert.NotEmpty(logsTest1);
            Assert.Equal(2, logsPage1.Count);
            Assert.Equal(2, logsPage2.Count);
            Assert.Equal(logsPage1.ToList()[0].Message, log4.Message);
            Assert.Equal(logsPage1.ToList()[1].Message, log3.Message);
            Assert.Equal(logsPage2.ToList()[0].Message, log2.Message);
            Assert.Equal(logsPage2.ToList()[1].Message, log1.Message);
        }
        [Fact]
        public async Task GetLogsPaginatedThreeLogsPerPageWithOnlyFourLogs_ShouldReturnTwoDiffPages()
        {
            // Arrange
            var log1 = new LogDto
            {
                Id = Guid.NewGuid(),
                Message = "log1",
                LogType = LogType.None,
                CreatedAt = DateTime.UtcNow,
            };
            var log2 = new LogDto
            {
                Id = Guid.NewGuid(),
                Message = "log2",
                LogType = LogType.Error,
                CreatedAt = DateTime.UtcNow,
            };
            var log3 = new LogDto
            {
                Id = Guid.NewGuid(),
                Message = "log3",
                LogType = LogType.Information,
                CreatedAt = DateTime.UtcNow,
            };
            var log4 = new LogDto
            {
                Id = Guid.NewGuid(),
                Message = "log4",
                LogType = LogType.Warning,
                CreatedAt = DateTime.UtcNow,

            };

            // Act

            await _logService.CreateLogAsync(log1);
            await _logService.CreateLogAsync(log2);
            await _logService.CreateLogAsync(log3);
            await _logService.CreateLogAsync(log4);

            var logsPage1 = (await _logService.GetLogsPaginatedAsync(3, 1)).ToList();
            var logsPage2 = (await _logService.GetLogsPaginatedAsync(3, 2)).ToList();

            // Assert

            Assert.Equal(3, logsPage1.Count);
            Assert.Single(logsPage2);
            Assert.Equal(logsPage1[0].Message, log4.Message);
            Assert.Equal(logsPage1[1].Message, log3.Message);
            Assert.Equal(logsPage1[2].Message, log2.Message);
            Assert.Equal(logsPage2[0].Message, log1.Message);

        }
        [Fact]
        public async Task GetPostLogsAsync_ShouldReturnLogsWithPostIdProvided()
        {
            // Arrange 
            var postId = Guid.NewGuid();
            var log1 = new LogDto
            {
                Id = Guid.NewGuid(),
                Message = "log1",
                LogType = LogType.None,
                PostId = postId,
            };
            var log2 = new LogDto
            {
                Id = Guid.NewGuid(),
                Message = "log2",
                LogType = LogType.None,
                PostId = Guid.NewGuid(),
            };
            var log3 = new LogDto
            {
                Id = Guid.NewGuid(),
                Message = "log3",
                LogType = LogType.None,
                PostId = Guid.NewGuid(),
            };
            var log4 = new LogDto
            {
                Id = Guid.NewGuid(),
                Message = "log4",
                LogType = LogType.None,
                PostId = postId,
            };
            // Act

            await _logService.CreateLogAsync(log1);
            await _logService.CreateLogAsync(log2);
            await _logService.CreateLogAsync(log3);
            await _logService.CreateLogAsync(log4);
            var logs = await _logService.GetPostLogsAsync(postId);


            // Assert

            Assert.Equal(2, logs.Count);
            Assert.Equal(logs.ToArray()[0].Message, log1.Message);
            Assert.Equal(logs.ToArray()[1].Message, log4.Message);
        }
        [Fact]
        public async Task GetUserLogsAsync_ShouldReturnLogsWithUserIdProvided()
        {
            // Arrange 
            var userId = Guid.NewGuid();
            var log1 = new LogDto
            {
                Id = Guid.NewGuid(),
                Message = "log1",
                LogType = LogType.None,
                UserId = userId,
            };
            var log2 = new LogDto
            {
                Id = Guid.NewGuid(),
                Message = "log2",
                LogType = LogType.None,
                UserId = Guid.NewGuid(),
            };
            var log3 = new LogDto
            {
                Id = Guid.NewGuid(),
                Message = "log3",
                LogType = LogType.None,
                UserId = Guid.NewGuid(),
            };
            var log4 = new LogDto
            {
                Id = Guid.NewGuid(),
                Message = "log4",
                LogType = LogType.None,
                UserId = userId,
            };
            // Act

            await _logService.CreateLogAsync(log1);
            await _logService.CreateLogAsync(log2);
            await _logService.CreateLogAsync(log3);
            await _logService.CreateLogAsync(log4);
            var logs = await _logService.GetUserLogsAsync(userId);


            // Assert

            Assert.Equal(2, logs.Count);
            Assert.Equal(logs.ToArray()[0].Message, log1.Message);
            Assert.Equal(logs.ToArray()[1].Message, log4.Message);
        }
    }
}
