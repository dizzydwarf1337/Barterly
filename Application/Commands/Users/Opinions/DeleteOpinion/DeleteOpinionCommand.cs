using Application.Core.MediatR.Requests;
using Application.Interfaces.CommandInterfaces;
using MediatR;

namespace Application.Commands.Users.Opinions.DeleteOpinion;

public class DeleteOpinionCommand : UserRequest<Unit>, IOpinionOwner
{
    public required Guid OpinionId { get; set; }
}