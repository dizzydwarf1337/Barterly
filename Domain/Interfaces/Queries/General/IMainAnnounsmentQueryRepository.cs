using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Queries.General
{
    public interface IMainAnnounsmentQueryRepository
    {

        Task<MainAnnounsment> GetMainAnnounsmentByIdAsync(Guid id);
        Task<List<MainAnnounsment>> GetAllMainAnnounsmentsAsync();
    }
}
