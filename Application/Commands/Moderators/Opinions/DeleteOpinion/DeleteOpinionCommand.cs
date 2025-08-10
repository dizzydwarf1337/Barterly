using Application.Core.MediatR.Requests;
using MediatR;

namespace Application.Commands.Moderators.Opinions.DeleteOpinion;

public class DeleteOpinionCommand : ModeratorRequest<Unit>
{
    public required Guid OpinionId { get; set; }
}