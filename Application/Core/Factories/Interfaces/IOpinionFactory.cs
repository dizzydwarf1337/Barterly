using Domain.Entities.Common;

namespace Application.Core.Factories.Interfaces;

public interface IOpinionFactory
{
    Task<Opinion> CreateOpinionAsync(Guid AuthorId, Guid SubjectId, string Content, int Rate, string OpinionType,
        CancellationToken token);
}