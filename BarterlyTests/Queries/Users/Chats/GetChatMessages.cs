using Application.Core.MediatR.Requests;
using Application.Queries.Users.Chat.GetChatMesages;
using Domain.Entities.Chat;
using Domain.Enums.Users;
using Domain.Interfaces.Queries.Chat;
using MockQueryable;
using Moq;

namespace BarterlyUnitTests.Queries.Users.Chats;

public class GetChatMessages
{
    private readonly Mock<IChatQueryRepository> _chatQueryRepositoryMock;
    private readonly Mock<IMessageQueryRepository> _messageQueryRepositoryMock;
    private readonly GetChatMessagesQueryHandler _handler;

    public GetChatMessages()
    {
        _chatQueryRepositoryMock = new Mock<IChatQueryRepository>();
        _messageQueryRepositoryMock = new Mock<IMessageQueryRepository>();
        _handler = new GetChatMessagesQueryHandler(
            _chatQueryRepositoryMock.Object,
            _messageQueryRepositoryMock.Object
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
    public async Task Handle_WhenChatExistsAndUserIsUser1_ReturnsMessages()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var chatId = Guid.NewGuid();
        var user2Id = Guid.NewGuid();

        var chat = new Domain.Entities.Chat.Chat
        {
            Id = chatId,
            User1 = userId,
            User2 = user2Id,
            CreatedAt = DateTime.UtcNow
        };

        var messages = new List<Message>
        {
            CreateMessage(Guid.NewGuid(), chatId, userId, user2Id, "Hello"),
            CreateMessage(Guid.NewGuid(), chatId, user2Id, userId, "Hi there")
        };

        var mockChats = new List<Domain.Entities.Chat.Chat> { chat }.AsQueryable().BuildMock();
        var mockMessages = messages.AsQueryable().BuildMock();

        _chatQueryRepositoryMock
            .Setup(x => x.GetChats())
            .Returns(mockChats);

        _messageQueryRepositoryMock
            .Setup(x => x.GetMessages())
            .Returns(mockMessages);

        var query = new GetChatMessagesQuery
        {
            ChatId = chatId,
            Page = 1,
            PageSize = 10,
            AuthorizeData = CreateAuthorizeData(userId)
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(2, result.Value.Items.Count);
        Assert.Equal(2, result.Value.TotalCount);
        Assert.Equal(1, result.Value.TotalPages);
    }

    [Fact]
    public async Task Handle_WhenChatExistsAndUserIsUser2_ReturnsMessages()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var chatId = Guid.NewGuid();
        var user1Id = Guid.NewGuid();

        var chat = new Domain.Entities.Chat.Chat
        {
            Id = chatId,
            User1 = user1Id,
            User2 = userId,
            CreatedAt = DateTime.UtcNow
        };

        var messages = new List<Message>
        {
            CreateMessage(Guid.NewGuid(), chatId, user1Id, userId, "Hello")
        };

        var mockChats = new List<Domain.Entities.Chat.Chat> { chat }.AsQueryable().BuildMock();
        var mockMessages = messages.AsQueryable().BuildMock();

        _chatQueryRepositoryMock
            .Setup(x => x.GetChats())
            .Returns(mockChats);

        _messageQueryRepositoryMock
            .Setup(x => x.GetMessages())
            .Returns(mockMessages);

        var query = new GetChatMessagesQuery
        {
            ChatId = chatId,
            Page = 1,
            PageSize = 10,
            AuthorizeData = CreateAuthorizeData(userId)
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Value.Items);
    }

    [Fact]
    public async Task Handle_WhenChatNotFound_ReturnsFailure()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var chatId = Guid.NewGuid();

        var emptyChats = new List<Domain.Entities.Chat.Chat>().AsQueryable().BuildMock();

        _chatQueryRepositoryMock
            .Setup(x => x.GetChats())
            .Returns(emptyChats);

        var query = new GetChatMessagesQuery
        {
            ChatId = chatId,
            Page = 1,
            PageSize = 10,
            AuthorizeData = CreateAuthorizeData(userId)
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Chat not found", result.Error);
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task Handle_WhenUserIsNotParticipant_ReturnsFailure()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var chatId = Guid.NewGuid();
        var user1Id = Guid.NewGuid();
        var user2Id = Guid.NewGuid();

        var chat = new Domain.Entities.Chat.Chat
        {
            Id = chatId,
            User1 = user1Id,
            User2 = user2Id,
            CreatedAt = DateTime.UtcNow
        };

        var mockChats = new List<Domain.Entities.Chat.Chat> { chat }.AsQueryable().BuildMock();

        _chatQueryRepositoryMock
            .Setup(x => x.GetChats())
            .Returns(mockChats);

        var query = new GetChatMessagesQuery
        {
            ChatId = chatId,
            Page = 1,
            PageSize = 10,
            AuthorizeData = CreateAuthorizeData(userId) // Different user
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Chat not found", result.Error);
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task Handle_WhenPaginationApplied_ReturnsCorrectPage()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var chatId = Guid.NewGuid();
        var user2Id = Guid.NewGuid();

        var chat = new Domain.Entities.Chat.Chat
        {
            Id = chatId,
            User1 = userId,
            User2 = user2Id,
            CreatedAt = DateTime.UtcNow
        };

        var messages = new List<Message>();
        for (int i = 0; i < 25; i++)
        {
            messages.Add(CreateMessage(Guid.NewGuid(), chatId, userId, user2Id, $"Message {i}"));
        }

        var mockChats = new List<Domain.Entities.Chat.Chat> { chat }.AsQueryable().BuildMock();
        var mockMessages = messages.AsQueryable().BuildMock();

        _chatQueryRepositoryMock
            .Setup(x => x.GetChats())
            .Returns(mockChats);

        _messageQueryRepositoryMock
            .Setup(x => x.GetMessages())
            .Returns(mockMessages);

        var query = new GetChatMessagesQuery
        {
            ChatId = chatId,
            Page = 2,
            PageSize = 10,
            AuthorizeData = CreateAuthorizeData(userId)
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(10, result.Value.Items.Count);
        Assert.Equal(25, result.Value.TotalCount);
        Assert.Equal(3, result.Value.TotalPages);
    }

    [Fact]
    public async Task Handle_MessagesSortedBySentAtAscending()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var chatId = Guid.NewGuid();
        var user2Id = Guid.NewGuid();

        var chat = new Domain.Entities.Chat.Chat
        {
            Id = chatId,
            User1 = userId,
            User2 = user2Id,
            CreatedAt = DateTime.UtcNow
        };

        var now = DateTime.UtcNow;
        var messages = new List<Message>
        {
            CreateMessage(Guid.NewGuid(), chatId, userId, user2Id, "Third", sentAt: now.AddMinutes(3)),
            CreateMessage(Guid.NewGuid(), chatId, userId, user2Id, "First", sentAt: now.AddMinutes(1)),
            CreateMessage(Guid.NewGuid(), chatId, userId, user2Id, "Second", sentAt: now.AddMinutes(2))
        };

        var mockChats = new List<Domain.Entities.Chat.Chat> { chat }.AsQueryable().BuildMock();
        var mockMessages = messages.AsQueryable().BuildMock();

        _chatQueryRepositoryMock
            .Setup(x => x.GetChats())
            .Returns(mockChats);

        _messageQueryRepositoryMock
            .Setup(x => x.GetMessages())
            .Returns(mockMessages);

        var query = new GetChatMessagesQuery
        {
            ChatId = chatId,
            Page = 1,
            PageSize = 10,
            AuthorizeData = CreateAuthorizeData(userId)
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("First", result.Value.Items[0].Content);
        Assert.Equal("Second", result.Value.Items[1].Content);
        Assert.Equal("Third", result.Value.Items[2].Content);
    }

    [Fact]
    public async Task Handle_WhenChatHasNoMessages_ReturnsEmptyResult()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var chatId = Guid.NewGuid();
        var user2Id = Guid.NewGuid();

        var chat = new Domain.Entities.Chat.Chat
        {
            Id = chatId,
            User1 = userId,
            User2 = user2Id,
            CreatedAt = DateTime.UtcNow
        };

        var emptyMessages = new List<Message>().AsQueryable().BuildMock();
        var mockChats = new List<Domain.Entities.Chat.Chat> { chat }.AsQueryable().BuildMock();

        _chatQueryRepositoryMock
            .Setup(x => x.GetChats())
            .Returns(mockChats);

        _messageQueryRepositoryMock
            .Setup(x => x.GetMessages())
            .Returns(emptyMessages);

        var query = new GetChatMessagesQuery
        {
            ChatId = chatId,
            Page = 1,
            PageSize = 10,
            AuthorizeData = CreateAuthorizeData(userId)
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Value.Items);
        Assert.Equal(0, result.Value.TotalCount);
        Assert.Equal(0, result.Value.TotalPages);
    }

    [Fact]
    public async Task Handle_MapsAllMessageFieldsCorrectly()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var chatId = Guid.NewGuid();
        var user2Id = Guid.NewGuid();
        var messageId = Guid.NewGuid();
        var postId = Guid.NewGuid();
        var sentAt = DateTime.UtcNow;
        var readAt = DateTime.UtcNow.AddMinutes(5);
        var acceptedAt = DateTime.UtcNow.AddMinutes(10);

        var chat = new Domain.Entities.Chat.Chat
        {
            Id = chatId,
            User1 = userId,
            User2 = user2Id,
            CreatedAt = DateTime.UtcNow
        };

        var message = new Message
        {
            Id = messageId,
            ChatId = chatId,
            Content = "Test message",
            Type = MessageType.Proposal,
            SenderId = userId,
            ReceiverId = user2Id,
            ReadBy = user2Id,
            SentAt = sentAt,
            ReadAt = readAt,
            AcceptedAt = acceptedAt,
            Price = 100.50m,
            IsAccepted = true,
            IsPaid = false,
            PostId = postId
        };

        var mockChats = new List<Domain.Entities.Chat.Chat> { chat }.AsQueryable().BuildMock();
        var mockMessages = new List<Message> { message }.AsQueryable().BuildMock();

        _chatQueryRepositoryMock
            .Setup(x => x.GetChats())
            .Returns(mockChats);

        _messageQueryRepositoryMock
            .Setup(x => x.GetMessages())
            .Returns(mockMessages);

        var query = new GetChatMessagesQuery
        {
            ChatId = chatId,
            Page = 1,
            PageSize = 10,
            AuthorizeData = CreateAuthorizeData(userId)
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        var resultMessage = result.Value.Items.First();
        Assert.Equal(messageId, resultMessage.Id);
        Assert.Equal(chatId, resultMessage.ChatId);
        Assert.Equal("Test message", resultMessage.Content);
        Assert.Equal(MessageType.Proposal, resultMessage.Type);
        Assert.Equal(userId, resultMessage.SenderId);
        Assert.Equal(user2Id, resultMessage.ReceiverId);
        Assert.Equal(user2Id, resultMessage.ReadBy);
        Assert.Equal(sentAt, resultMessage.SentAt);
        Assert.Equal(readAt, resultMessage.ReadAt);
        Assert.Equal(acceptedAt, resultMessage.AcceptedAt);
        Assert.Equal(100.50m, resultMessage.Price);
        Assert.True(resultMessage.IsAccepted);
        Assert.False(resultMessage.IsPaid);
        Assert.Equal(postId, resultMessage.PostId);
    }

    [Fact]
    public async Task Handle_CalculatesTotalPagesCorrectly()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var chatId = Guid.NewGuid();
        var user2Id = Guid.NewGuid();

        var chat = new Domain.Entities.Chat.Chat
        {
            Id = chatId,
            User1 = userId,
            User2 = user2Id,
            CreatedAt = DateTime.UtcNow
        };

        var messages = new List<Message>();
        for (int i = 0; i < 23; i++)
        {
            messages.Add(CreateMessage(Guid.NewGuid(), chatId, userId, user2Id, $"Message {i}"));
        }

        var mockChats = new List<Domain.Entities.Chat.Chat> { chat }.AsQueryable().BuildMock();
        var mockMessages = messages.AsQueryable().BuildMock();

        _chatQueryRepositoryMock
            .Setup(x => x.GetChats())
            .Returns(mockChats);

        _messageQueryRepositoryMock
            .Setup(x => x.GetMessages())
            .Returns(mockMessages);

        var query = new GetChatMessagesQuery
        {
            ChatId = chatId,
            Page = 1,
            PageSize = 10,
            AuthorizeData = CreateAuthorizeData(userId)
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(23, result.Value.TotalCount);
        Assert.Equal(3, result.Value.TotalPages); // Ceiling(23/10) = 3
    }

    [Fact]
    public async Task Handle_OnlyReturnsMessagesFromSpecificChat()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var chatId = Guid.NewGuid();
        var otherChatId = Guid.NewGuid();
        var user2Id = Guid.NewGuid();

        var chat = new Domain.Entities.Chat.Chat
        {
            Id = chatId,
            User1 = userId,
            User2 = user2Id,
            CreatedAt = DateTime.UtcNow
        };

        var messages = new List<Message>
        {
            CreateMessage(Guid.NewGuid(), chatId, userId, user2Id, "Chat 1 Message"),
            CreateMessage(Guid.NewGuid(), otherChatId, userId, user2Id, "Other Chat Message"),
            CreateMessage(Guid.NewGuid(), chatId, userId, user2Id, "Chat 1 Message 2")
        };

        var mockChats = new List<Domain.Entities.Chat.Chat> { chat }.AsQueryable().BuildMock();
        var mockMessages = messages.AsQueryable().BuildMock();

        _chatQueryRepositoryMock
            .Setup(x => x.GetChats())
            .Returns(mockChats);

        _messageQueryRepositoryMock
            .Setup(x => x.GetMessages())
            .Returns(mockMessages);

        var query = new GetChatMessagesQuery
        {
            ChatId = chatId,
            Page = 1,
            PageSize = 10,
            AuthorizeData = CreateAuthorizeData(userId)
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.Items.Count);
        Assert.All(result.Value.Items, msg => Assert.Equal(chatId, msg.ChatId));
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