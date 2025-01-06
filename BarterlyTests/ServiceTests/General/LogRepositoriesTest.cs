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

namespace YourNamespace.Tests
{
    public class LogRepositoriesTests
    {
        private readonly Mock<ILogCommandRepository> _logCommandRepositoryMock;
        private readonly Mock<ILogQueryRepository> _logQueryRepositoryMock;

        public LogRepositoriesTests()
        {
            _logCommandRepositoryMock = new Mock<ILogCommandRepository>();
            _logQueryRepositoryMock = new Mock<ILogQueryRepository>();
        }

        [Fact]
        public async Task CreateLogWithMessageProvidedOnly_ShouldCallCreateLogAsync()
        {
            // Arrange
            var log = new Log { Message = "Message" };

            _logCommandRepositoryMock
                .Setup(repo => repo.CreateLogAsync(It.IsAny<Log>()))
                .Returns(Task.CompletedTask);

            // Act
            await _logCommandRepositoryMock.Object.CreateLogAsync(log);

            // Assert
            _logCommandRepositoryMock.Verify(repo => repo.CreateLogAsync(It.Is<Log>(l =>
                l.Message == "Message" &&
                l.LogType == LogType.None &&
                l.UserId == null &&
                l.PostId == null
            )), Times.Once);
        }
        [Fact]
        public async Task ChangeLogTypeAsync_ShouldChangeLogTypeWithTypeProvided()
        {
            //Arrange 
            var log = new Log { Id = Guid.NewGuid(), LogType = LogType.None };
            _logCommandRepositoryMock
                .Setup(repo => repo.ChangeLogType(It.IsAny<Guid>(), It.IsAny<LogType>()))
                .Returns(Task.CompletedTask);
            // Act
            await _logCommandRepositoryMock.Object.ChangeLogType(log.Id, LogType.Error);
            // Assert
            _logCommandRepositoryMock.Verify(repo => repo.ChangeLogType(It.Is<Guid>(id => id == log.Id), It.Is<LogType>(type => type == LogType.Error)), Times.Once);
        }
    }
}
