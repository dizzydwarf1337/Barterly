using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Commands.Post;
using Domain.Interfaces.Queries.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PostOpinionService : IOpinionService
    {

        private readonly IPostOpinionCommandRepository _postOpinionCommandRepository;
        private readonly IPostOpinionQueryRepository _postOpinionQueryRepository;
        private readonly ILogService _logService;
        private readonly IMapper _mapper;
        public PostOpinionService(IPostOpinionCommandRepository postOpinionCommandRepository, IPostOpinionQueryRepository postOpinionQueryRepository, ILogService logService, IMapper mapper)
        {
            _postOpinionCommandRepository = postOpinionCommandRepository;
            _postOpinionQueryRepository = postOpinionQueryRepository;
            _logService = logService;
            _mapper = mapper;
        }

        public async Task AddOpinion(OpinionDto opinionDto)
        {
            var opinion = _mapper.Map<PostOpinion>(opinionDto);
            await _postOpinionCommandRepository.CreatePostOpinionAsync(opinion);
            await _logService.CreateLogAsync($"Opinion created with id {opinion.Id} ",LogType.Information,null,opinion.PostId,opinion.AuthorId);
        }

        public async Task DeleteOpinion(Guid opinionId)
        {
            await _postOpinionCommandRepository.DeletePostOpinionAsync(opinionId);
            await _logService.CreateLogAsync($"Opinion deleted with id {opinionId} ", LogType.Information, null, null, null);
        }

        public async Task<OpinionDto> GetOpinionById(Guid opinionId)
        {
            var opinion = await _postOpinionQueryRepository.GetPostOpinionByIdAsync(opinionId);
            return _mapper.Map<OpinionDto>(opinion);
        }

        public async Task<ICollection<OpinionDto>> GetOpinions()
        {
            return _mapper.Map<ICollection<OpinionDto>>(await _postOpinionQueryRepository.GetPostOpinionsAsync());
        }

        public async Task<ICollection<OpinionDto>> GetOpinionsBySubjectId(Guid subjectId)
        {
            return _mapper.Map<ICollection<OpinionDto>>(await _postOpinionQueryRepository.GetPostOpinionsByPostIdAsync(subjectId));
        }

        public async Task<ICollection<OpinionDto>> GetOpinionsByAuthorId(Guid userId)
        {
            return _mapper.Map<ICollection<OpinionDto>>(await _postOpinionQueryRepository.GetPostOpinionsByAuthorIdAsync(userId));
        }

        public async Task SetOpinionIsHidden(Guid opinionId, bool isHidden)
        {
            await _postOpinionCommandRepository.SetHiddenPostOpinionAsync(opinionId, isHidden);
        }

        public async Task UpdateOpinion(OpinionDto opinionDto)
        {
            var opinion = _mapper.Map<PostOpinion>(opinionDto);
            await _postOpinionCommandRepository.UpdatePostOpinionAsync(opinion);
            await _logService.CreateLogAsync($"Opinion updated with id {opinion.Id} ", LogType.Information, null, opinion.PostId, opinion.AuthorId);
        }

        public async Task<ICollection<OpinionDto>> GetOpinionsPaginated(Guid subjectId, int page, int pageSize)
        {
            return _mapper.Map<ICollection<OpinionDto>>(await _postOpinionQueryRepository.GetPostOpinionsPaginatedAsync(subjectId, page, pageSize));
        }
    }
}
