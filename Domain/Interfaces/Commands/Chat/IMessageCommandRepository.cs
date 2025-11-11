using Domain.Entities.Chat;

namespace Domain.Interfaces.Commands.Chat;

public interface IMessageCommandRepository
{
    public Task SaveMessage(Message message, CancellationToken token);
    public Task UpdateMessage(Message message, CancellationToken token);
}