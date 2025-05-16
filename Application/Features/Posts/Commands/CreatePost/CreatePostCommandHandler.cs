using API.Core.ApiResponse;
using Application.DTOs.Posts;
using Application.Features.Posts.Events.PostCreatedEvent;
using Application.Interfaces;
using Application.Services;
using Domain.Enums.Common;
using Domain.Exceptions.BusinessExceptions;
using Domain.Exceptions.DataExceptions;
using MediatR;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Posts.Commands.CreatePost
{
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, ApiResponse<PostDto>>
    {
        private readonly IPostService _postService;
        private readonly IMediator _mediator;
        private readonly IUserSettingsService _userSettingsService;
        private readonly ILogService _logService;
        public CreatePostCommandHandler(IPostService postService, IMediator mediator,IUserSettingsService userSettingsService,ILogService logService)
        {
            _postService = postService;
            _mediator = mediator;
            _userSettingsService = userSettingsService;
            _logService = logService;
        }

        public async Task<ApiResponse<PostDto>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {

            try
            {
                if (await IsUserPostRestricted(request.post.OwnerId))
                {
                    return ApiResponse<PostDto>.Failure("User is restricted from posting");
                }
                var post = await _postService.CreatePost(request.post);

                
                Console.WriteLine("Sending an event");
                await _mediator.Publish(new PostCreatedEvent
                {
                    UserId = Guid.Parse(post.OwnerId),
                    PostId = Guid.Parse(post.Id)
                });
                await _logService.CreateLogAsync($"Post created title: {post.Title}", LogType.Information,postId:Guid.Parse(post.Id),userId:Guid.Parse(post.OwnerId));
                return ApiResponse<PostDto>.Success(post, 201);

            }
            catch (InvalidDataProvidedException ex)
            {   
                return ApiResponse<PostDto>.Failure(ex.Message);
            } 
            catch (EntityCreatingException ex)
            {
                return ApiResponse<PostDto>.Failure(ex.Message,409);
            }
            catch (Exception ex) 
            {
                Console.WriteLine(request.post.Title + " " + request.post.FullDescription + " " + request.post.City);
                Console.WriteLine("Error: " + ex.Message + ex.StackTrace);
                return ApiResponse<PostDto>.Failure("Error while creating new post");
            }
        }
        private async Task<bool> IsUserPostRestricted(string userId)
        {
            var userSettings = await _userSettingsService.GetUserSettingByUserIdAsync(Guid.Parse(userId));
            return userSettings.IsPostRestricted;
        }
    }
}
