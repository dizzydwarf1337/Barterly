using Domain.Entities.Common;

namespace Domain.Interfaces.Queries.General;

public interface IMainAnnounsmentQueryRepository
{
    Task<MainAnnounsment> GetMainAnnounsmentByIdAsync(Guid id, CancellationToken token);
    Task<ICollection<MainAnnounsment>> GetAllMainAnnounsmentsAsync(CancellationToken token);
}