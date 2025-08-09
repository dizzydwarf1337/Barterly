using Application.Core.MediatR.Requests;
using Application.Interfaces.CommandInterfaces;

namespace Application.Commands.Users.Opinions.EditOpinion;

public class EditOpinionCommand : UserRequest<EditOpinionCommand.Result>, IHasOwner, IOpinionOwner,
    IMessageContainer
{
    public required string OpinionType { get; set; }
    public int Rate { get; set; }
    public required Guid OwnerId { get; set; }
    public required string Message { get; set; }
    public required Guid OpinionId { get; set; }

    public record Result(
        Guid Id,
        Guid AuthorId,
        Guid SubjectId,
        string Content,
        int Rate,
        bool IsHidden,
        DateTime CreatedAt,
        DateTime? LastUpdatedAt);
}