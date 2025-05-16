using Application.DTOs.Posts;
using Application.Interfaces;
using Domain.Entities.Posts;
using Domain.Interfaces.Commands.Post;
using Domain.Interfaces.Queries.Post;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PostSettingsService : IPostSettingsService
    {
        private readonly IPostSettingsCommandRepository _postSettingsCommandRepository;
        private readonly IPostSettingsQueryRepository _postSettingsQueryRepository;

        public PostSettingsService(IPostSettingsCommandRepository postSettingsCommandRepository, IPostSettingsQueryRepository postSettingsQueryRepository)
        {
            _postSettingsCommandRepository = postSettingsCommandRepository;
            _postSettingsQueryRepository = postSettingsQueryRepository;
        }

        public async Task ApprovePost(ApprovePostDto approvePostDto)
        {
            var postSettings = await _postSettingsQueryRepository.GetPostSettingsByPostId(Guid.Parse(approvePostDto.postId));
            postSettings.IsHidden = false;
            postSettings.postStatusType = Domain.Enums.Posts.PostStatusType.Published;
            postSettings.RejectionMessage = null;
            await _postSettingsCommandRepository.UpdatePostSettings(postSettings);
        }

        public async Task DeletePost(Guid postId)
        {
            var postSettings = await _postSettingsQueryRepository.GetPostSettingsByPostId(postId);
            postSettings.IsDeleted = true;
            postSettings.IsHidden = true;
            await _postSettingsCommandRepository.UpdatePostSettings(postSettings);
        }

        public async Task RejectPost(RejectPostDto rejectPostDto)
        {
            var postSettings = await _postSettingsQueryRepository.GetPostSettingsByPostId(Guid.Parse(rejectPostDto.postId));
            postSettings.IsHidden = true;
            postSettings.postStatusType = Domain.Enums.Posts.PostStatusType.Rejected;
            postSettings.RejectionMessage = rejectPostDto.reason;
            await _postSettingsCommandRepository.UpdatePostSettings(postSettings);
        }

        public async Task SetPostHidden(Guid postId, bool value)
        {
            var postSettings = await _postSettingsQueryRepository.GetPostSettingsByPostId(postId);
            postSettings.IsHidden = value;
            await _postSettingsCommandRepository.UpdatePostSettings(postSettings);

        }
        public async Task ResubmitPost(Guid postId)
        {
            var postSettings = await _postSettingsQueryRepository.GetPostSettingsByPostId(postId);
            postSettings.IsHidden = true;
            postSettings.postStatusType = Domain.Enums.Posts.PostStatusType.ReSubmitted;
            await _postSettingsCommandRepository.UpdatePostSettings(postSettings);
        }
    }
}
