using Application.Core.MediatR.Requests;
using MediatR;

namespace Application.Commands.Admins.Opinions.SetOpinionHidden;

public class SetOpinionHiddenCommand : AdminRequest<Unit>
{
    public required Guid OpinionId { get; set; }
    public bool IsHidden { get; set; }
}