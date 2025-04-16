using Domain.Entities.Posts;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Commands.Post
{
    public interface IReportPostCommandRepository
    {
        Task CreateReportPostAsync(ReportPost report);
        Task DeleteReportPostAsync(Guid reportPostId);
        Task ChangeReportPostStatusAsync(Guid id, ReportStatusType status);
    }
}
