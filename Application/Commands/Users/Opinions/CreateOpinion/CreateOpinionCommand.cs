using Application.Core.MediatR.Requests;
using Application.Interfaces.CommandInterfaces;

namespace Application.Commands.Users.Opinions.CreateOpinion;

public class CreateOpinionCommand : UserRequest<CreateOpinionCommand.Result>, IHasOwner, IMessageContainer
{
    public required Guid SubjectId { get; set; }
    public int Rate { get; set; }
    public required string OpinionType { get; set; }
    public required Guid OwnerId { get; set; }
    public required string Message { get; set; }

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