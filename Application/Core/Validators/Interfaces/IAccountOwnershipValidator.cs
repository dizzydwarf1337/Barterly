using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.Validators.Interfaces
{
    public interface IAccountOwnershipValidator
    {
      Task ValidateAccountOwnership(Guid userId, Guid accountId);
    }
}
