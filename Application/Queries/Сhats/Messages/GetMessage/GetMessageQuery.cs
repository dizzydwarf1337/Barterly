using Domain.Entities.Chat;
using MediatR;

namespace Application.Queries.Сhats.Messages.GetMessage;

public class GetMessageQuery : IRequest<Message>
{
    public Guid Id { get; set; }
}