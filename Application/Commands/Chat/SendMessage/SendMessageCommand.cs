using Application.Hub;
using Domain.Entities.Chat;
using MediatR;

namespace Application.Commands.Chat.SendMessage;

public class SendMessageCommand : IRequest<Message>
{
    public HubDto.CommonMessage Message { get; set; }    
}