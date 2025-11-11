using Application.Hub;
using MediatR;

namespace Application.Commands.Chat.AcceptPropose;

public class AcceptProposeCommand : IRequest<Unit>
{
    public HubDto.AcceptProposal AcceptPropose { get; set; }
}