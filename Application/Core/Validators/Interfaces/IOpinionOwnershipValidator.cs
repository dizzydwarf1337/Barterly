using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.Validators.Interfaces
{
    public interface IOpinionOwnershipValidator
    {
     public Task ValidateOpinionOwnership(Guid userId, Guid opinionId);
    }
}
