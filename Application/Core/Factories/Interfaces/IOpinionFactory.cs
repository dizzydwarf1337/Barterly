using Application.DTOs.General.Opinions;
using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.Factories.Interfaces
{
    public interface IOpinionFactory
    {
        Task<Opinion> CreateOpinionAsync(CreateOpinionDto opinionDto);
    }
}
