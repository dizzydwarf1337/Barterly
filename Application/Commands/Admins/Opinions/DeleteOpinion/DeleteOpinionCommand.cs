using Application.Core.MediatR.Requests;
using MediatR;

namespace Application.Commands.Admins.Opinions.DeleteOpinion;

public class DeleteOpinionCommand : AdminRequest<Unit>
{
    public required Guid OpinionId { get; set; }
}