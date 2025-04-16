using Domain.Entities.Common;

namespace Domain.Interfaces.Queries.General
{
    public interface IMainAnnounsmentQueryRepository
    {

        Task<MainAnnounsment> GetMainAnnounsmentByIdAsync(Guid id);
        Task<List<MainAnnounsment>> GetAllMainAnnounsmentsAsync();
    }
}
