using Application.Core.MediatR.Requests;
using Application.Queries.Users.Chat.GetMyChats;
using Domain.Entities.Chat;
using Domain.Enums.Users;
using Domain.Interfaces.Queries.Chat;
using Domain.Interfaces.Queries.User;
using MockQueryable;
using Moq;
using Chat = Domain.Entities.Chat.Chat;
using User = Domain.Entities.Users.User;
namespace BarterlyUnitTests.Queries.Users.Chats;

public class GetMyChats
{
    private readonly Mock<IChatQueryRepository> _chatQueryRepositoryMock;
    private readonly Mock<IUserQueryRepository> _userQueryRepositoryMock;
    private readonly GetMyChatsQueryHandler _handler;

    public GetMyChats()
    {
        _chatQueryRepositoryMock = new Mock<IChatQueryRepository>();
        _userQueryRepositoryMock = new Mock<IUserQueryRepository>();
        _handler = new GetMyChatsQueryHandler(
            _chatQueryRepositoryMock.Object,
            _userQueryRepositoryMock.Object
        );
    }

    private AuthorizeData CreateAuthorizeData(Guid userId)
    {
        return new AuthorizeData(
            userId,
            new List<UserRoles> { UserRoles.User },
            "test-token",
            null
        );
    }

    [Fact]
    public async Task Handle_WhenUserHasChatsAsUser1_ReturnsChats()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();
        var chatId = Guid.NewGuid();

        var chats = new List<Chat>
        {
            new Chat
            {
                Id = chatId,
                User1 = userId,
                User2 = otherUserId,
                CreatedAt = DateTime.UtcNow,
                Messages = new List<Message>
                {
                    CreateMessage(Guid.NewGuid(), chatId, userId, otherUserId, "Hello")
                }
            }
        };

        var users = new List<User>
        {
            new User
            {
                Id = userId,
                FirstName = "John",
                LastName = "Doe",
                ProfilePicturePath = "/path/john.jpg"
            },
            new User
            {
                Id = otherUserId,
                FirstName = "Jane",
                LastName = "Smith",
                ProfilePicturePath = "/path/jane.jpg"
            }
        };

        var mockChats = chats.AsQueryable().BuildMock();
        var mockUsers = users.AsQueryable().BuildMock();

        _chatQueryRepositoryMock
            .Setup(x => x.GetChats())
            .Returns(mockChats);

        _userQueryRepositoryMock
            .Setup(x => x.GetUsers())
            .Returns(mockUsers);

        var query = new GetMyChatsQuery
        {
            AuthorizeData = CreateAuthorizeData(userId)
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Single(result.Value);
        
        var chat = result.Value.First();
        Assert.Equal(chatId, chat.Id);
        Assert.Equal(userId, chat.User1.UserId);
        Assert.Equal(otherUserId, chat.User2.UserId);
    }

    [Fact]
    public async Task Handle_WhenUserHasChatsAsUser2_ReturnsChats()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();
        var chatId = Guid.NewGuid();

        var chats = new List<Chat>
        {
            new Chat
            {
                Id = chatId,
                User1 = otherUserId,
                User2 = userId,
                CreatedAt = DateTime.UtcNow,
                Messages = new List<Message>()
            }
        };

        var users = new List<User>
        {
            new User
            {
                Id = userId,
                FirstName = "John",
                LastName = "Doe",
                ProfilePicturePath = null
            },
            new User
            {
                Id = otherUserId,
                FirstName = "Jane",
                LastName = "Smith",
                ProfilePicturePath = null
            }
        };

        var mockChats = chats.AsQueryable().BuildMock();
        var mockUsers = users.AsQueryable().BuildMock();

        _chatQueryRepositoryMock
            .Setup(x => x.GetChats())
            .Returns(mockChats);

        _userQueryRepositoryMock
            .Setup(x => x.GetUsers())
            .Returns(mockUsers);

        var query = new GetMyChatsQuery
        {
            AuthorizeData = CreateAuthorizeData(userId)
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Value);
        
        var chat = result.Value.First();
        Assert.Equal(otherUserId, chat.User1.UserId);
        Assert.Equal(userId, chat.User2.UserId);
    }

    [Fact]
    public async Task Handle_WhenUserHasMultipleChats_ReturnsAllChats()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user2Id = Guid.NewGuid();
        var user3Id = Guid.NewGuid();
        var chat1Id = Guid.NewGuid();
        var chat2Id = Guid.NewGuid();

        var chats = new List<Chat>
        {
            new Chat
            {
                Id = chat1Id,
                User1 = userId,
                User2 = user2Id,
                CreatedAt = DateTime.UtcNow,
                Messages = new List<Message>()
            },
            new Chat
            {
                Id = chat2Id,
                User1 = user3Id,
                User2 = userId,
                CreatedAt = DateTime.UtcNow,
                Messages = new List<Message>()
            }
        };

        var users = new List<User>
        {
            new User { Id = userId, FirstName = "User", LastName = "One" },
            new User { Id = user2Id, FirstName = "User", LastName = "Two" },
            new User { Id = user3Id, FirstName = "User", LastName = "Three" }
        };

        var mockChats = chats.AsQueryable().BuildMock();
        var mockUsers = users.AsQueryable().BuildMock();

        _chatQueryRepositoryMock
            .Setup(x => x.GetChats())
            .Returns(mockChats);

        _userQueryRepositoryMock
            .Setup(x => x.GetUsers())
            .Returns(mockUsers);

        var query = new GetMyChatsQuery
        {
            AuthorizeData = CreateAuthorizeData(userId)
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.Count);
    }

    [Fact]
    public async Task Handle_WhenUserHasNoChats_ReturnsEmptyCollection()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var emptyChats = new List<Chat>();
        var emptyUsers = new List<User>();

        var mockChats = emptyChats.AsQueryable().BuildMock();
        var mockUsers = emptyUsers.AsQueryable().BuildMock();

        _chatQueryRepositoryMock
            .Setup(x => x.GetChats())
            .Returns(mockChats);

        _userQueryRepositoryMock
            .Setup(x => x.GetUsers())
            .Returns(mockUsers);

        var query = new GetMyChatsQuery
        {
            AuthorizeData = CreateAuthorizeData(userId)
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Empty(result.Value);
    }

    [Fact]
    public async Task Handle_WhenChatHasNoMessages_ReturnsEmptyMessagesList()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();
        var chatId = Guid.NewGuid();

        var chats = new List<Chat>
        {
            new Chat
            {
                Id = chatId,
                User1 = userId,
                User2 = otherUserId,
                CreatedAt = DateTime.UtcNow,
                Messages = new List<Message>()
            }
        };

        var users = new List<User>
        {
            new User { Id = userId, FirstName = "John", LastName = "Doe" },
            new User { Id = otherUserId, FirstName = "Jane", LastName = "Smith" }
        };

        var mockChats = chats.AsQueryable().BuildMock();
        var mockUsers = users.AsQueryable().BuildMock();

        _chatQueryRepositoryMock
            .Setup(x => x.GetChats())
            .Returns(mockChats);

        _userQueryRepositoryMock
            .Setup(x => x.GetUsers())
            .Returns(mockUsers);

        var query = new GetMyChatsQuery
        {
            AuthorizeData = CreateAuthorizeData(userId)
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        var chat = result.Value.First();
        Assert.Empty(chat.Messages);
    }

    [Fact]
    public async Task Handle_ReturnsOnlyLastMessage()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();
        var chatId = Guid.NewGuid();
        var now = DateTime.UtcNow;

        var chats = new List<Chat>
        {
            new Chat
            {
                Id = chatId,
                User1 = userId,
                User2 = otherUserId,
                CreatedAt = DateTime.UtcNow,
                Messages = new List<Message>
                {
                    CreateMessage(Guid.NewGuid(), chatId, userId, otherUserId, "First", sentAt: now.AddMinutes(-10)),
                    CreateMessage(Guid.NewGuid(), chatId, otherUserId, userId, "Second", sentAt: now.AddMinutes(-5)),
                    CreateMessage(Guid.NewGuid(), chatId, userId, otherUserId, "Last", sentAt: now)
                }
            }
        };

        var users = new List<User>
        {
            new User { Id = userId, FirstName = "John", LastName = "Doe" },
            new User { Id = otherUserId, FirstName = "Jane", LastName = "Smith" }
        };

        var mockChats = chats.AsQueryable().BuildMock();
        var mockUsers = users.AsQueryable().BuildMock();

        _chatQueryRepositoryMock
            .Setup(x => x.GetChats())
            .Returns(mockChats);

        _userQueryRepositoryMock
            .Setup(x => x.GetUsers())
            .Returns(mockUsers);

        var query = new GetMyChatsQuery
        {
            AuthorizeData = CreateAuthorizeData(userId)
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        var chat = result.Value.First();
        Assert.Single(chat.Messages);
        Assert.Equal("Last", chat.Messages.First().Content);
    }

    [Fact]
    public async Task Handle_MapsUserFieldsCorrectly()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();
        var chatId = Guid.NewGuid();

        var chats = new List<Chat>
        {
            new Chat
            {
                Id = chatId,
                User1 = userId,
                User2 = otherUserId,
                CreatedAt = DateTime.UtcNow,
                Messages = new List<Message>()
            }
        };

        var users = new List<User>
        {
            new User
            {
                Id = userId,
                FirstName = "John",
                LastName = "Doe",
                ProfilePicturePath = "/images/john.jpg"
            },
            new User
            {
                Id = otherUserId,
                FirstName = "Jane",
                LastName = "Smith",
                ProfilePicturePath = "/images/jane.jpg"
            }
        };

        var mockChats = chats.AsQueryable().BuildMock();
        var mockUsers = users.AsQueryable().BuildMock();

        _chatQueryRepositoryMock
            .Setup(x => x.GetChats())
            .Returns(mockChats);

        _userQueryRepositoryMock
            .Setup(x => x.GetUsers())
            .Returns(mockUsers);

        var query = new GetMyChatsQuery
        {
            AuthorizeData = CreateAuthorizeData(userId)
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        var chat = result.Value.First();
        
        Assert.Equal("John", chat.User1.FirstName);
        Assert.Equal("Doe", chat.User1.LastName);
        Assert.Equal("/images/john.jpg", chat.User1.ImagePath);
        
        Assert.Equal("Jane", chat.User2.FirstName);
        Assert.Equal("Smith", chat.User2.LastName);
        Assert.Equal("/images/jane.jpg", chat.User2.ImagePath);
    }

    [Fact]
    public async Task Handle_WhenUserHasNullProfilePicture_ReturnsNull()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();
        var chatId = Guid.NewGuid();

        var chats = new List<Chat>
        {
            new Chat
            {
                Id = chatId,
                User1 = userId,
                User2 = otherUserId,
                CreatedAt = DateTime.UtcNow,
                Messages = new List<Message>()
            }
        };

        var users = new List<User>
        {
            new User
            {
                Id = userId,
                FirstName = "John",
                LastName = "Doe",
                ProfilePicturePath = null
            },
            new User
            {
                Id = otherUserId,
                FirstName = "Jane",
                LastName = "Smith",
                ProfilePicturePath = null
            }
        };

        var mockChats = chats.AsQueryable().BuildMock();
        var mockUsers = users.AsQueryable().BuildMock();

        _chatQueryRepositoryMock
            .Setup(x => x.GetChats())
            .Returns(mockChats);

        _userQueryRepositoryMock
            .Setup(x => x.GetUsers())
            .Returns(mockUsers);

        var query = new GetMyChatsQuery
        {
            AuthorizeData = CreateAuthorizeData(userId)
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        var chat = result.Value.First();
        Assert.Null(chat.User1.ImagePath);
        Assert.Null(chat.User2.ImagePath);
    }

    [Fact]
    public async Task Handle_MapsChatFieldsCorrectly()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();
        var chatId = Guid.NewGuid();
        var createdAt = DateTime.UtcNow.AddDays(-5);

        var chats = new List<Chat>
        {
            new Chat
            {
                Id = chatId,
                User1 = userId,
                User2 = otherUserId,
                CreatedAt = createdAt,
                Messages = new List<Message>()
            }
        };

        var users = new List<User>
        {
            new User { Id = userId, FirstName = "John", LastName = "Doe" },
            new User { Id = otherUserId, FirstName = "Jane", LastName = "Smith" }
        };

        var mockChats = chats.AsQueryable().BuildMock();
        var mockUsers = users.AsQueryable().BuildMock();

        _chatQueryRepositoryMock
            .Setup(x => x.GetChats())
            .Returns(mockChats);

        _userQueryRepositoryMock
            .Setup(x => x.GetUsers())
            .Returns(mockUsers);

        var query = new GetMyChatsQuery
        {
            AuthorizeData = CreateAuthorizeData(userId)
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        var chat = result.Value.First();
        Assert.Equal(chatId, chat.Id);
        Assert.Equal(createdAt, chat.CreatedAt);
    }

    [Fact]
    public async Task Handle_MapsLastMessageFieldsCorrectly()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();
        var chatId = Guid.NewGuid();
        var messageId = Guid.NewGuid();
        var postId = Guid.NewGuid();
        var sentAt = DateTime.UtcNow;
        var readAt = DateTime.UtcNow.AddMinutes(5);

        var message = new Message
        {
            Id = messageId,
            ChatId = chatId,
            Content = "Test message",
            Type = MessageType.Proposal,
            SenderId = userId,
            ReceiverId = otherUserId,
            ReadBy = otherUserId,
            SentAt = sentAt,
            ReadAt = readAt,
            AcceptedAt = null,
            Price = 150.75m,
            IsAccepted = true,
            IsPaid = false,
            PostId = postId
        };

        var chats = new List<Chat>
        {
            new Chat
            {
                Id = chatId,
                User1 = userId,
                User2 = otherUserId,
                CreatedAt = DateTime.UtcNow,
                Messages = new List<Message> { message }
            }
        };

        var users = new List<User>
        {
            new User { Id = userId, FirstName = "John", LastName = "Doe" },
            new User { Id = otherUserId, FirstName = "Jane", LastName = "Smith" }
        };

        var mockChats = chats.AsQueryable().BuildMock();
        var mockUsers = users.AsQueryable().BuildMock();

        _chatQueryRepositoryMock
            .Setup(x => x.GetChats())
            .Returns(mockChats);

        _userQueryRepositoryMock
            .Setup(x => x.GetUsers())
            .Returns(mockUsers);

        var query = new GetMyChatsQuery
        {
            AuthorizeData = CreateAuthorizeData(userId)
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        var chat = result.Value.First();
        var lastMessage = chat.Messages.First();
        
        Assert.Equal(messageId, lastMessage.Id);
        Assert.Equal(chatId, lastMessage.ChatId);
        Assert.Equal("Test message", lastMessage.Content);
        Assert.Equal(MessageType.Proposal, lastMessage.Type);
        Assert.Equal(userId, lastMessage.SenderId);
        Assert.Equal(otherUserId, lastMessage.ReceiverId);
        Assert.Equal(otherUserId, lastMessage.ReadBy);
        Assert.Equal(sentAt, lastMessage.SentAt);
        Assert.Equal(readAt, lastMessage.ReadAt);
        Assert.Equal(150.75m, lastMessage.Price);
        Assert.True(lastMessage.IsAccepted);
        Assert.False(lastMessage.IsPaid);
        Assert.Equal(postId, lastMessage.PostId);
    }

    [Fact]
    public async Task Handle_OnlyReturnsChatsWhereUserIsParticipant()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user2Id = Guid.NewGuid();
        var user3Id = Guid.NewGuid();
        var chat1Id = Guid.NewGuid();
        var chat2Id = Guid.NewGuid();

        var chats = new List<Chat>
        {
            new Chat
            {
                Id = chat1Id,
                User1 = userId,
                User2 = user2Id,
                CreatedAt = DateTime.UtcNow,
                Messages = new List<Message>()
            },
            new Chat
            {
                Id = chat2Id,
                User1 = user2Id,
                User2 = user3Id,
                CreatedAt = DateTime.UtcNow,
                Messages = new List<Message>()
            }
        };

        var users = new List<User>
        {
            new User { Id = userId, FirstName = "User", LastName = "One" },
            new User { Id = user2Id, FirstName = "User", LastName = "Two" },
            new User { Id = user3Id, FirstName = "User", LastName = "Three" }
        };

        var mockChats = chats.AsQueryable().BuildMock();
        var mockUsers = users.AsQueryable().BuildMock();

        _chatQueryRepositoryMock
            .Setup(x => x.GetChats())
            .Returns(mockChats);

        _userQueryRepositoryMock
            .Setup(x => x.GetUsers())
            .Returns(mockUsers);

        var query = new GetMyChatsQuery
        {
            AuthorizeData = CreateAuthorizeData(userId)
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Value);
        Assert.Equal(chat1Id, result.Value.First().Id);
    }

    // Helper method
    private Message CreateMessage(
        Guid id,
        Guid chatId,
        Guid senderId,
        Guid receiverId,
        string content,
        DateTime? sentAt = null)
    {
        return new Message
        {
            Id = id,
            ChatId = chatId,
            Content = content,
            Type = MessageType.Common,
            SenderId = senderId,
            ReceiverId = receiverId,
            SentAt = sentAt ?? DateTime.UtcNow,
            ReadBy = null,
            ReadAt = null,
            AcceptedAt = null,
            Price = null,
            IsAccepted = null,
            IsPaid = null,
            PostId = null
        };
    }
}