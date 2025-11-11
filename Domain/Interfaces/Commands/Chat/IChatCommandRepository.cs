namespace Domain.Interfaces.Commands.Chat;

public interface IChatCommandRepository
{
    public Task<Guid> CreateChat(Entities.Chat.Chat chat, CancellationToken token);
}