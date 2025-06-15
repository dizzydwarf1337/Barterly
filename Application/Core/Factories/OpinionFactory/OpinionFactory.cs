using Application.Core.Factories.Interfaces;
using Application.DTOs.General.Opinions;
using AutoMapper;
using Azure.Core;
using Domain.Entities.Common;
using Domain.Entities.Posts;
using Domain.Entities.Users;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Commands.Post;
using Domain.Interfaces.Commands.User;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.Factories.OpinionFactory
{
    public class OpinionFactory : IOpinionFactory
    {
        private readonly IUserOpinionCommandRepository _userOpinionCommandRepository;
        private readonly IPostOpinionCommandRepository _postOpinionCommandRepository;
        private readonly IMapper _mapper;

        public OpinionFactory(IUserOpinionCommandRepository userOpinionCommandRepository, IPostOpinionCommandRepository postOpinionCommandRepository, IMapper mapper)
        {
            _userOpinionCommandRepository = userOpinionCommandRepository;
            _postOpinionCommandRepository = postOpinionCommandRepository;
            _mapper = mapper;
        }

        public async Task<Opinion> CreateOpinionAsync(CreateOpinionDto opinionDto)
        {

            Opinion opinionDb;
            if(opinionDto.OpinionType == "Post")
            {
                var opinion = _mapper.Map<PostOpinion>(opinionDto);

                opinionDb = await _postOpinionCommandRepository.CreatePostOpinionAsync(opinion);
            }
            else if (opinionDto.OpinionType == "User")
            {
                var opinion = _mapper.Map<UserOpinion>(opinionDto);
                opinionDb = await _userOpinionCommandRepository.CreateUserOpinionAsync(opinion);
            }
            else
            {
                throw new EntityCreatingException("Opinion","OpinionFactory");
            }
            return opinionDb;

        }
    }
}
