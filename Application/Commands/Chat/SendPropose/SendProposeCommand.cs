using Application.Hub;
using MediatR;

namespace Application.Commands.Chat.SendPropose;

public class SendProposeCommand : IRequest<Unit>
{
    public HubDto.ProposalMessage Message { get; set; }
}