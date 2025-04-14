using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Commands.User;
using Domain.Interfaces.Queries.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserOpinionService : IOpinionService
    {
        private readonly IUserOpinionCommandRepository _userOpinionCommandRepository;
        private readonly IUserOpinionQueryRepository _userOpinionQueryRepository;
        private readonly IMapper _mapper;
        private readonly ILogService _logService;

        public UserOpinionService(IUserOpinionCommandRepository userOpinionCommandRepository, IUserOpinionQueryRepository userOpinionQueryRepository, IMapper mapper, ILogService logService)
        {
            _userOpinionCommandRepository = userOpinionCommandRepository;
            _userOpinionQueryRepository = userOpinionQueryRepository;
            _mapper = mapper;
            _logService = logService;
        }

        public async Task AddOpinion(OpinionDto opinionDto)
        {
            var opinion = _mapper.Map<UserOpinion>(opinionDto);
            await _userOpinionCommandRepository.CreateUserOpinionAsync(opinion);
            await _logService.CreateLogAsync($"Opinion created with id {opinion.Id} ", LogType.Information, null, null, opinion.UserId);
        }

        public async Task DeleteOpinion(Guid opinionId)
        {
            await _userOpinionCommandRepository.DeleteUserOpinionAsync(opinionId);
            await _logService.CreateLogAsync($"Opinion deleted with id {opinionId} ", LogType.Information, null, null, null);
        }

        public async Task<OpinionDto> GetOpinionById(Guid opinionId)
        {
            return _mapper.Map<OpinionDto>(await _userOpinionQueryRepository.GetUserOpinionByIdAsync(opinionId));
        }

        public async Task<ICollection<OpinionDto>> GetOpinions()
        {
            return _mapper.Map<ICollection<OpinionDto>>(await _userOpinionQueryRepository.GetUserOpinionsAsync());
        }

        public async Task<ICollection<OpinionDto>> GetOpinionsByAuthorId(Guid userId)
        {
            return _mapper.Map<ICollection<OpinionDto>>(await _userOpinionQueryRepository.GetUserOpinionsByAuthorIdAsync(userId));
        }

        public async Task<ICollection<OpinionDto>> GetOpinionsBySubjectId(Guid subjectId)
        {
            return _mapper.Map<ICollection<OpinionDto>>(await _userOpinionQueryRepository.GetUserOpinionsByUserIdAsync(subjectId));
        }

        public async Task<ICollection<OpinionDto>> GetOpinionsPaginated(Guid subjectId, int page, int pageSize)
        {
            return _mapper.Map<ICollection<OpinionDto>>(await _userOpinionQueryRepository.GetUserOpinionsPaginated(subjectId, page, pageSize));
        }

        public async Task SetOpinionIsHidden(Guid opinionId, bool isHidden)
        {
            await _userOpinionCommandRepository.SetHiddenUserOpinionAsync(opinionId, isHidden);
            await _logService.CreateLogAsync("Opinion hidden status changed", LogType.Information, null, null, null);
        }

        public async Task UpdateOpinion(OpinionDto opinionDto)
        {
            var opinion = _mapper.Map<UserOpinion>(opinionDto);
            await _userOpinionCommandRepository.UpdateUserOpinionAsync(opinion);
            await _logService.CreateLogAsync($"Opinion updated with id {opinion.Id} ", LogType.Information, null, null, opinion.AuthorId);
        }
    }
}
