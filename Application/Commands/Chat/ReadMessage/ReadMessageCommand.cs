using Application.Hub;
using MediatR;

namespace Application.Commands.Chat.ReadMessage;

public class ReadMessageCommand : IRequest<Unit>
{
    public HubDto.ReadMessage ReadMessage { get; set; }
}