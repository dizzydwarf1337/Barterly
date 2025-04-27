using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.Validators.Interfaces
{
    public interface IOwnershipValidator
    {

        public Task ValidatePostOwnership(Guid userId, Guid postId);
        public Task ValidateAccountOwnership(Guid userId, Guid accountId);
        public Task ValidateOpinionOwnership(Guid userId, Guid opinionId);
        public Task ValidateReportOwnership(Guid userId, Guid reportId);


    }
}
