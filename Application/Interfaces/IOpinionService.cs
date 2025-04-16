using Application.DTOs;

namespace Application.Interfaces
{
    public interface IOpinionService
    {
        Task AddOpinion(OpinionDto opinion);
        Task DeleteOpinion(Guid opinionId);
        Task UpdateOpinion(OpinionDto opinion);

        Task SetOpinionIsHidden(Guid opinionId, bool isHidden);

        Task<ICollection<OpinionDto>> GetOpinions();
        Task<OpinionDto> GetOpinionById(Guid opinionId);
        Task<ICollection<OpinionDto>> GetOpinionsByAuthorId(Guid userId);
        Task<ICollection<OpinionDto>> GetOpinionsBySubjectId(Guid subjectId);
        Task<ICollection<OpinionDto>> GetOpinionsPaginated(Guid subjectId, int page, int pageSize);
    }
}
