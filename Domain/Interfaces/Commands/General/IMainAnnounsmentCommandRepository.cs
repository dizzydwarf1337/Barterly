using Domain.Entities.Common;

namespace Domain.Interfaces.Commands.General
{
    public interface IMainAnnounsmentCommandRepository
    {
        Task CreateMainAnnounsmentAsync(MainAnnounsment mainAnnounsment);
        Task UpdateMainAnnounsmentAsync(MainAnnounsment mainAnnounsment);
        Task UploadImageAsync(Guid id, string imageUrl);
        Task DeleteMainAnnounsmentAsync(Guid id);
    }
}
