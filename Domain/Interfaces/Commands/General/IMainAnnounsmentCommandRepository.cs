using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
