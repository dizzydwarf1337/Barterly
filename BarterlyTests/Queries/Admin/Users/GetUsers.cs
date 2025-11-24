using Application.Queries.Admins.Users.GetUsers;
using Domain.Entities.Users;
using Domain.Enums.Users;
using Domain.Interfaces.Queries.User;
using Microsoft.AspNetCore.Identity;
using MockQueryable;
using Moq;

namespace BarterlyUnitTests.Queries.Admin.Users;

public class GetUsersQueryHandlerTests
{
    private readonly Mock<IUserQueryRepository> _userQueryRepositoryMock;
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly GetUsersQueryHandler _handler;

    public GetUsersQueryHandlerTests()
    {
        _userQueryRepositoryMock = new Mock<IUserQueryRepository>();
        
        var userStoreMock = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(
            userStoreMock.Object, null, null, null, null, null, null, null, null);
        
        _handler = new GetUsersQueryHandler(
            _userQueryRepositoryMock.Object,
            _userManagerMock.Object
        );
    }

    [Fact]
    public async Task Handle_WhenValidRequest_ReturnsPaginatedResults()
    {
        // Arrange
        var users = CreateTestUsers(15);
        var mockUsers = users.AsQueryable().BuildMock();

        _userQueryRepositoryMock
            .Setup(x => x.GetUsers())
            .Returns(mockUsers);

        _userManagerMock
            .Setup(x => x.IsInRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(false);

        var query = new GetUsersQuery
        {
            FilterBy = new GetUsersQuery.FilterSpecification
            {
                PageSize = 10,
                PageNumber = 1
            }
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(15, result.Value.TotalCount);
        Assert.Equal(1, result.Value.TotalPages);
        Assert.Equal(10, result.Value.Items.Count);
    }

    [Fact]
    public async Task Handle_WhenSearchFilterApplied_ReturnsMatchingUsers()
    {
        // Arrange
        var users = new List<User>
        {
            CreateTestUser("Alice", "Smith", "alice@example.com"),
            CreateTestUser("Bob", "Smith", "bob@example.com"),
            CreateTestUser("Charlie", "Johnson", "charlie@example.com")
        };
        var mockUsers = users.AsQueryable().BuildMock();

        _userQueryRepositoryMock
            .Setup(x => x.GetUsers())
            .Returns(mockUsers);

        _userManagerMock
            .Setup(x => x.IsInRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(false);

        var query = new GetUsersQuery
        {
            FilterBy = new GetUsersQuery.FilterSpecification
            {
                Search = "Smith",
                PageSize = 10,
                PageNumber = 1
            }
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Value.TotalCount >= 1);
        Assert.True(result.Value.Items.Count >= 1);
        Assert.All(result.Value.Items, item => 
            Assert.True(item.FirstName.Contains("Smith") || 
                        item.LastName.Contains("Smith") || 
                        item.Email.Contains("Smith")));
    }

    [Fact]
    public async Task Handle_WhenSearchByEmail_ReturnsMatchingUsers()
    {
        // Arrange
        var users = new List<User>
        {
            CreateTestUser("John", "Doe", "john@test.com"),
            CreateTestUser("Jane", "Smith", "jane@example.com"),
            CreateTestUser("Bob", "Johnson", "bob@test.com")
        };
        var mockUsers = users.AsQueryable().BuildMock();

        _userQueryRepositoryMock
            .Setup(x => x.GetUsers())
            .Returns(mockUsers);

        _userManagerMock
            .Setup(x => x.IsInRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(false);

        var query = new GetUsersQuery
        {
            FilterBy = new GetUsersQuery.FilterSpecification
            {
                Search = "test.com",
                PageSize = 10,
                PageNumber = 1
            }
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.TotalCount);
    }

    [Fact]
    public async Task Handle_WhenIsBannedFilterApplied_ReturnsBannedUsers()
    {
        // Arrange
        var users = new List<User>
        {
            CreateTestUserWithSettings(isBanned: true),
            CreateTestUserWithSettings(isBanned: false),
            CreateTestUserWithSettings(isBanned: true)
        };
        var mockUsers = users.AsQueryable().BuildMock();

        _userQueryRepositoryMock
            .Setup(x => x.GetUsers())
            .Returns(mockUsers);

        _userManagerMock
            .Setup(x => x.IsInRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(false);

        var query = new GetUsersQuery
        {
            FilterBy = new GetUsersQuery.FilterSpecification
            {
                IsBanned = true,
                PageSize = 10,
                PageNumber = 1
            }
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.TotalCount);
    }

    [Fact]
    public async Task Handle_WhenIsDeletedFilterApplied_ReturnsDeletedUsers()
    {
        // Arrange
        var users = new List<User>
        {
            CreateTestUserWithSettings(isDeleted: true),
            CreateTestUserWithSettings(isDeleted: false),
            CreateTestUserWithSettings(isDeleted: true)
        };
        var mockUsers = users.AsQueryable().BuildMock();

        _userQueryRepositoryMock
            .Setup(x => x.GetUsers())
            .Returns(mockUsers);

        _userManagerMock
            .Setup(x => x.IsInRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(false);

        var query = new GetUsersQuery
        {
            FilterBy = new GetUsersQuery.FilterSpecification
            {
                IsDeleted = true,
                PageSize = 10,
                PageNumber = 1
            }
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.TotalCount);
    }

    [Fact]
    public async Task Handle_WhenIsEmailConfirmedFilterApplied_ReturnsConfirmedUsers()
    {
        // Arrange
        var users = new List<User>
        {
            CreateTestUserWithEmailConfirmed(true),
            CreateTestUserWithEmailConfirmed(false),
            CreateTestUserWithEmailConfirmed(true)
        };
        var mockUsers = users.AsQueryable().BuildMock();

        _userQueryRepositoryMock
            .Setup(x => x.GetUsers())
            .Returns(mockUsers);

        _userManagerMock
            .Setup(x => x.IsInRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(false);

        var query = new GetUsersQuery
        {
            FilterBy = new GetUsersQuery.FilterSpecification
            {
                IsEmailConfirmed = true,
                PageSize = 10,
                PageNumber = 1
            }
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.TotalCount);
    }

    [Theory]
    [InlineData("createdat", false)]
    [InlineData("createdat", true)]
    [InlineData("lastseen", false)]
    [InlineData("lastseen", true)]
    public async Task Handle_WhenSortByApplied_ReturnsSortedUsers(string sortBy, bool isDescending)
    {
        // Arrange
        var users = new List<User>
        {
            CreateTestUserWithDates(DateTime.UtcNow.AddDays(-3), DateTime.UtcNow.AddHours(-5)),
            CreateTestUserWithDates(DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddHours(-1)),
            CreateTestUserWithDates(DateTime.UtcNow.AddDays(-2), DateTime.UtcNow.AddHours(-3))
        };
        var mockUsers = users.AsQueryable().BuildMock();

        _userQueryRepositoryMock
            .Setup(x => x.GetUsers())
            .Returns(mockUsers);

        _userManagerMock
            .Setup(x => x.IsInRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(false);

        var query = new GetUsersQuery
        {
            FilterBy = new GetUsersQuery.FilterSpecification
            {
                PageSize = 10,
                PageNumber = 1
            },
            SortBy = new GetUsersQuery.SortSpecification
            {
                SortBy = sortBy,
                IsDescending = isDescending
            }
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(3, result.Value.Items.Count);
    }

    [Fact]
    public async Task Handle_WhenUserIsAdmin_ReturnsAdminRole()
    {
        // Arrange
        var user = CreateTestUser("Admin", "User", "admin@example.com");
        var users = new List<User> { user };
        var mockUsers = users.AsQueryable().BuildMock();

        _userQueryRepositoryMock
            .Setup(x => x.GetUsers())
            .Returns(mockUsers);

        _userManagerMock
            .Setup(x => x.IsInRoleAsync(user, "Admin"))
            .ReturnsAsync(true);

        _userManagerMock
            .Setup(x => x.IsInRoleAsync(user, "Moderator"))
            .ReturnsAsync(false);

        var query = new GetUsersQuery
        {
            FilterBy = new GetUsersQuery.FilterSpecification
            {
                PageSize = 10,
                PageNumber = 1
            }
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Value.Items);
        Assert.Equal(UserRoles.Admin, result.Value.Items.First().Role);
    }

    [Fact]
    public async Task Handle_WhenUserIsModerator_ReturnsModeratorRole()
    {
        // Arrange
        var user = CreateTestUser("Moderator", "User", "mod@example.com");
        var users = new List<User> { user };
        var mockUsers = users.AsQueryable().BuildMock();

        _userQueryRepositoryMock
            .Setup(x => x.GetUsers())
            .Returns(mockUsers);

        _userManagerMock
            .Setup(x => x.IsInRoleAsync(user, "Admin"))
            .ReturnsAsync(false);

        _userManagerMock
            .Setup(x => x.IsInRoleAsync(user, "Moderator"))
            .ReturnsAsync(true);

        var query = new GetUsersQuery
        {
            FilterBy = new GetUsersQuery.FilterSpecification
            {
                PageSize = 10,
                PageNumber = 1
            }
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Value.Items);
        Assert.Equal(UserRoles.Moderator, result.Value.Items.First().Role);
    }

    [Fact]
    public async Task Handle_WhenUserIsRegular_ReturnsUserRole()
    {
        // Arrange
        var user = CreateTestUser("Regular", "User", "user@example.com");
        var users = new List<User> { user };
        var mockUsers = users.AsQueryable().BuildMock();

        _userQueryRepositoryMock
            .Setup(x => x.GetUsers())
            .Returns(mockUsers);

        _userManagerMock
            .Setup(x => x.IsInRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(false);

        var query = new GetUsersQuery
        {
            FilterBy = new GetUsersQuery.FilterSpecification
            {
                PageSize = 10,
                PageNumber = 1
            }
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Value.Items);
        Assert.Equal(UserRoles.User, result.Value.Items.First().Role);
    }

    [Fact]
    public async Task Handle_WhenMultipleFiltersApplied_ReturnsMatchingUsers()
    {
        // Arrange
        var users = new List<User>
        {
            CreateTestUserWithAllFilters("John", "Doe", "john@test.com", true, false, true),
            CreateTestUserWithAllFilters("Jane", "Smith", "jane@example.com", false, false, true),
            CreateTestUserWithAllFilters("Bob", "Johnson", "bob@test.com", true, false, true)
        };
        var mockUsers = users.AsQueryable().BuildMock();

        _userQueryRepositoryMock
            .Setup(x => x.GetUsers())
            .Returns(mockUsers);

        _userManagerMock
            .Setup(x => x.IsInRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(false);

        var query = new GetUsersQuery
        {
            FilterBy = new GetUsersQuery.FilterSpecification
            {
                Search = "test.com",
                IsBanned = true,
                IsEmailConfirmed = true,
                PageSize = 10,
                PageNumber = 1
            }
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.TotalCount);
    }

    [Fact]
    public async Task Handle_WhenPaginationApplied_ReturnsCorrectPage()
    {
        // Arrange
        var users = CreateTestUsers(25);
        var mockUsers = users.AsQueryable().BuildMock();

        _userQueryRepositoryMock
            .Setup(x => x.GetUsers())
            .Returns(mockUsers);

        _userManagerMock
            .Setup(x => x.IsInRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(false);

        var query = new GetUsersQuery
        {
            FilterBy = new GetUsersQuery.FilterSpecification
            {
                PageSize = 10,
                PageNumber = 2
            }
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(25, result.Value.TotalCount);
        Assert.Equal(2, result.Value.TotalPages);
        Assert.Equal(10, result.Value.Items.Count);
    }

    [Fact]
    public async Task Handle_WhenNoUsersMatch_ReturnsEmptyResult()
    {
        // Arrange
        var users = new List<User>();
        var mockUsers = users.AsQueryable().BuildMock();

        _userQueryRepositoryMock
            .Setup(x => x.GetUsers())
            .Returns(mockUsers);

        var query = new GetUsersQuery
        {
            FilterBy = new GetUsersQuery.FilterSpecification
            {
                PageSize = 10,
                PageNumber = 1
            }
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(0, result.Value.TotalCount);
        Assert.Equal(0, result.Value.TotalPages);
        Assert.Empty(result.Value.Items);
    }

    // Helper methods
    private List<User> CreateTestUsers(int count)
    {
        var users = new List<User>();
        for (int i = 0; i < count; i++)
        {
            users.Add(CreateTestUser($"User{i}", $"Test{i}", $"user{i}@example.com"));
        }
        return users;
    }

    private User CreateTestUser(string firstName, string lastName, string email)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            CreatedAt = DateTime.UtcNow,
            LastSeen = DateTime.UtcNow,
            EmailConfirmed = false,
            Setting = new UserSettings
            {
                Id = Guid.NewGuid(),
                IsHidden = false,
                IsDeleted = false,
                IsBanned = false
            }
        };
    }

    private User CreateTestUserWithSettings(bool isBanned = false, bool isDeleted = false)
    {
        var user = CreateTestUser("Test", "User", "test@example.com");
        user.Setting.IsBanned = isBanned;
        user.Setting.IsDeleted = isDeleted;
        return user;
    }

    private User CreateTestUserWithEmailConfirmed(bool emailConfirmed)
    {
        var user = CreateTestUser("Test", "User", "test@example.com");
        user.EmailConfirmed = emailConfirmed;
        return user;
    }

    private User CreateTestUserWithDates(DateTime createdAt, DateTime lastSeen)
    {
        var user = CreateTestUser("Test", "User", "test@example.com");
        user.CreatedAt = createdAt;
        user.LastSeen = lastSeen;
        return user;
    }

    private User CreateTestUserWithAllFilters(string firstName, string lastName, string email, 
        bool isBanned, bool isDeleted, bool emailConfirmed)
    {
        var user = CreateTestUser(firstName, lastName, email);
        user.Setting.IsBanned = isBanned;
        user.Setting.IsDeleted = isDeleted;
        user.EmailConfirmed = emailConfirmed;
        return user;
    }
}