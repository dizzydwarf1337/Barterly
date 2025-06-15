using API.Core.ApiResponse;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Commands.Post;
using Domain.Interfaces.Commands.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Opinions.Commands.DeleteOpinion
{
    public class DeleteOpinionCommandHandler : IRequestHandler<DeleteOpinionCommand, ApiResponse<Unit>>
    {
        private readonly IUserOpinionCommandRepository _userOpinionCommandRepository;
        private readonly IPostOpinionCommandRepository _postOpinionCommandRepository;

        public DeleteOpinionCommandHandler(IUserOpinionCommandRepository userOpinionCommandRepository, IPostOpinionCommandRepository postOpinionCommandRepository)
        {
            _userOpinionCommandRepository = userOpinionCommandRepository;
            _postOpinionCommandRepository = postOpinionCommandRepository;
        }

        public async Task<ApiResponse<Unit>> Handle(DeleteOpinionCommand request, CancellationToken cancellationToken)
        {
            var opinionId = Guid.Parse(request.OpinionId);
            try
            {
                await _postOpinionCommandRepository.DeletePostOpinionAsync(opinionId);
            }
            catch (EntityNotFoundException)
            {
                await _userOpinionCommandRepository.DeleteUserOpinionAsync(opinionId);
            }

            return ApiResponse<Unit>.Success(Unit.Value);
        }
    }
}
