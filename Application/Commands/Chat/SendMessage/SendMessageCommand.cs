using Application.Hub;
using MediatR;

namespace Application.Commands.Chat.SendMessage;

public class SendMessageCommand : IRequest<Unit>
{
    public HubDto.CommonMessage Message { get; set; }    
}