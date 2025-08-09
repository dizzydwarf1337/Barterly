using Domain.Entities.Common;

namespace Domain.Interfaces.Commands.General;

public interface IMainAnnounsmentCommandRepository
{
    Task CreateMainAnnounsmentAsync(MainAnnounsment mainAnnounsment, CancellationToken token);
    Task UpdateMainAnnounsmentAsync(MainAnnounsment mainAnnounsment, CancellationToken token);
    Task UploadImageAsync(Guid id, string imageUrl, CancellationToken token);
    Task DeleteMainAnnounsmentAsync(Guid id, CancellationToken token);
}