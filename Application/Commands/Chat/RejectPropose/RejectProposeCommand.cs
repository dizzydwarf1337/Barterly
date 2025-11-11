using Application.Core.MediatR.Requests;
using Application.Hub;
using MediatR;

namespace Application.Commands.Chat.RejectPropose;

public class RejectProposeCommand : IRequest<Unit>
{
    public HubDto.RejectProposal Reject { get; set; }
}