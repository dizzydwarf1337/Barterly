using API.Core.ApiResponse;
using Application.DTOs.General.Opinions;
using AutoMapper;
using Domain.Interfaces.Commands.Post;
using Domain.Interfaces.Commands.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Opinions.Commands.EditOpinion
{
    public class EditOpinionCommandHandler : IRequestHandler<EditOpinionCommand, ApiResponse<OpinionDto>>
    {
        private readonly IPostOpinionCommandRepository _postOpinionCommandRepository;
        private readonly IUserOpinionCommandRepository _userOpinionCommandRepository;
        private readonly IMapper _mapper;

        public EditOpinionCommandHandler(IPostOpinionCommandRepository postOpinionCommandRepository, IUserOpinionCommandRepository userOpinionCommandRepository, IMapper mapper)
        {
            _postOpinionCommandRepository = postOpinionCommandRepository;
            _userOpinionCommandRepository = userOpinionCommandRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<OpinionDto>> Handle(EditOpinionCommand request, CancellationToken cancellationToken)
        {
            if(request.EditOpinionDto.OpinionType == "User")
            {
                return ApiResponse<OpinionDto>.Success(
                    _mapper.Map<OpinionDto>(
                        await _userOpinionCommandRepository.UpdateUserOpinionAsync(Guid.Parse(request.EditOpinionDto.Id), request.EditOpinionDto.Content, request.EditOpinionDto.Rate)
                        )
                    );

            }
            else if (request.EditOpinionDto.OpinionType == "Post")
            {
                return ApiResponse<OpinionDto>.Success(
                    _mapper.Map<OpinionDto>(
                        await _postOpinionCommandRepository.UpdatePostOpinionAsync(Guid.Parse(request.EditOpinionDto.Id),request.EditOpinionDto.Content,request.EditOpinionDto.Rate)
                        )
                    );
            }
            else
            {
                throw new ArgumentException("Invalid Opinion Type");
            }
        }
    }
}
