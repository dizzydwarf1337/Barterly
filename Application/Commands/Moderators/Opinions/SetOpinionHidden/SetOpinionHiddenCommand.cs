using Application.Core.MediatR.Requests;
using MediatR;

namespace Application.Commands.Moderators.Opinions.SetOpinionHidden;

public class SetOpinionHiddenCommand : ModeratorRequest<Unit>
{
    public Guid OpinionId { get; set; }
    public bool IsHidden { get; set; }
}