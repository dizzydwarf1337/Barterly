using Application.Core.Validators.Interfaces;
using Domain.Entities.Common;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Queries.Post;
using Domain.Interfaces.Queries.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.Validators.Implementation
{
    public class OpinionOwnershipValidator : IOpinionOwnershipValidator
    {
        private readonly IUserOpinionQueryRepository _userOpinionQueryRepository;
        private readonly IPostOpinionQueryRepository _postOpinionQueryRepository;

        public OpinionOwnershipValidator(IUserOpinionQueryRepository userOpinionQueryRepository, IPostOpinionQueryRepository postOpinionQueryRepository)
        {
            _userOpinionQueryRepository = userOpinionQueryRepository;
            _postOpinionQueryRepository = postOpinionQueryRepository;
        }

        public async Task ValidateOpinionOwnership(Guid userId, Guid opinionId)
        {
            Opinion? opinion = null;
            try
            {
              opinion = await _userOpinionQueryRepository.GetUserOpinionByIdAsync(opinionId);
            }
            catch 
            {
              opinion = await _postOpinionQueryRepository.GetPostOpinionByIdAsync(opinionId); 
            }
            if (opinion.AuthorId != userId) throw new AccessForbiddenException("ValidateOpinionOwnerShip", userId.ToString(), "AuthorId and userId mismatch");

        }
    }
}
