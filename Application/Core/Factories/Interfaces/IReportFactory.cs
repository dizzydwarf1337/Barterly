using Application.DTOs.Reports;
using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.Factories.Interfaces
{
    public interface IReportFactory
    {
        Task CreateReportAsync(CreateReportDto createReportDto); 
    }
}
