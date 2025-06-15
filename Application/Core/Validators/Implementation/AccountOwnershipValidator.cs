using Application.Core.Validators.Interfaces;
using Domain.Exceptions.BusinessExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.Validators.Implementation
{
    public class AccountOwnershipValidator : IAccountOwnershipValidator
    {
        public Task ValidateAccountOwnership(Guid userId, Guid accountId)
        {
            return userId == accountId ? Task.CompletedTask : throw new AccessForbiddenException("ValidateAccountOwnership", userId.ToString(), "User id and Account id doesn't match");
        }
    }
}
