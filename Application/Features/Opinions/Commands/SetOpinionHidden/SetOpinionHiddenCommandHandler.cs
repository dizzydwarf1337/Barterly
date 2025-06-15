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

namespace Application.Features.Opinions.Commands.SetOpinionHidden
{
    public class SetOpinionHiddenCommandHandler : IRequestHandler<SetOpinionHiddenCommand, ApiResponse<Unit>>
    {
        private readonly IUserOpinionCommandRepository _userOpinionCommandRepository;
        private readonly IPostOpinionCommandRepository _postOpinionCommandRepository;

        public SetOpinionHiddenCommandHandler(IUserOpinionCommandRepository userOpinionCommandRepository, IPostOpinionCommandRepository postOpinionCommandRepository)
        {
            _userOpinionCommandRepository = userOpinionCommandRepository;
            _postOpinionCommandRepository = postOpinionCommandRepository;
        }

        public async Task<ApiResponse<Unit>> Handle(SetOpinionHiddenCommand request, CancellationToken cancellationToken)
        {
            var subjectId = Guid.Parse(request.HideOpinionDto.OpinionId);
            try
            {
                await _postOpinionCommandRepository.SetHiddenPostOpinionAsync(subjectId, request.HideOpinionDto.isHidden);
            }
            catch (EntityNotFoundException)
            {
                await _userOpinionCommandRepository.SetHiddenUserOpinionAsync(subjectId, request.HideOpinionDto.isHidden);
            }

            return ApiResponse<Unit>.Success(Unit.Value);
        }
    }
}
