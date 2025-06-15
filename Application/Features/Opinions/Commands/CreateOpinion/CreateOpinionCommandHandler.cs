using API.Core.ApiResponse;
using Application.Core.Factories.Interfaces;
using Application.DTOs.General.Opinions;
using Application.Features.Posts.Events.PostOpinionCreatedEvent;
using Application.Features.Users.Events.UserOpinionCreated;
using AutoMapper;
using Domain.Entities.Posts;
using Domain.Entities.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Opinions.Commands.CreateOpinion
{
    public class CreateOpinionCommandHandler : IRequestHandler<CreateOpinionCommand, ApiResponse<OpinionDto>>
    {
        private readonly IOpinionFactory _opinionFactory;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public CreateOpinionCommandHandler(IOpinionFactory opinionFactory, IMediator mediator, IMapper mapper)
        {
            _opinionFactory = opinionFactory;
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<ApiResponse<OpinionDto>> Handle(CreateOpinionCommand request, CancellationToken cancellationToken)
        {
            var opinion = await _opinionFactory.CreateOpinionAsync(request.createOpinionDto);
            switch (request.createOpinionDto.OpinionType)
            {
                case "User":
                    await _mediator.Publish(new UserOpinionCreatedEvent { opinion = (UserOpinion)opinion });
                    break;
                case "Post":
                    await _mediator.Publish(new PostOpinionCreatedEvent { PostOpinion = (PostOpinion)opinion });
                    break;
            }
            return ApiResponse<OpinionDto>.Success(_mapper.Map<OpinionDto>(opinion),201);
        }
    }
}
