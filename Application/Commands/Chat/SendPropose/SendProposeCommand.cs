using Application.Hub;
using Domain.Entities.Chat;
using MediatR;

namespace Application.Commands.Chat.SendPropose;

public class SendProposeCommand : IRequest<Message>
{
    public HubDto.ProposalMessage Message { get; set; }
}