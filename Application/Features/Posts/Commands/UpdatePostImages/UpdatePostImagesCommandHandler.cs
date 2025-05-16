using API.Core.ApiResponse;
using Application.DTOs.Posts;
using Application.Features.Posts.Events.PostUpdatedEvent;
using Application.Interfaces;
using Application.Services;
using Domain.Enums.Common;
using Domain.Exceptions.BusinessExceptions;
using Domain.Exceptions.DataExceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Posts.Commands.UpdatePostImages
{
    public class UpdatePostImagesCommandHandler : IRequestHandler<UpdatePostImagesCommand, ApiResponse<PostDto>>
    {
        private readonly IPostService _postService;
        private readonly IMediator _mediator;
        private readonly IUserSettingsService _userSettingsService;
        private readonly ILogService _logService;

        public UpdatePostImagesCommandHandler(IPostService postService, IMediator mediator, IUserSettingsService userSettingsService, ILogService logService)
        {
            _postService = postService;
            _mediator = mediator;
            _userSettingsService = userSettingsService;
            _logService = logService;
        }

        public async Task<ApiResponse<PostDto>> Handle(UpdatePostImagesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (await IsUserPostRestricted(request.ImagesDto.UserId))
                {
                    return ApiResponse<PostDto>.Failure("User is restricted from posting");
                }
                var post = await _postService.UploadImages(request.ImagesDto);
                await _mediator.Publish(new PostUpdatedEvent { PostId = post.Id, UserId=post.OwnerId});
                await _logService.CreateLogAsync($"Post updated title: {post.Title}", LogType.Information, postId: Guid.Parse(post.Id), userId: Guid.Parse(post.OwnerId));
                return ApiResponse<PostDto>.Success(post, 200);
            }
            catch (InvalidDataProvidedException ex)
            {
                return ApiResponse<PostDto>.Failure(ex.Message);
            }
            catch (EntityNotFoundException ex)
            {
                return ApiResponse<PostDto>.Failure(ex.Message, 404);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message + ex.StackTrace);
                return ApiResponse<PostDto>.Failure("Error while updating post");
            }
        }
        private async Task<bool> IsUserPostRestricted(string userId)
        {
            var userSettings = await _userSettingsService.GetUserSettingByUserIdAsync(Guid.Parse(userId));
            return userSettings.IsPostRestricted;
        }
    }
}
