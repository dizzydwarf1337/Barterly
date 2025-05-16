using Application.Core.Validators.Interfaces;
using Application.Interfaces;
using Application.Services;
using Domain.Exceptions.BusinessExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.Validators.Implementation
{
    public class OwnershipValidator : IOwnershipValidator
    {
        private readonly IUserService _userService;
        private readonly IPostOpinionService _postOpinionService;
        private readonly IUserOpinionService _userOpinionService;
        private readonly IPostService _postService;
        private readonly IUserReportService _userReportService;
        private readonly IPostReportService _postReportService;

        public OwnershipValidator(
            IUserService userService, 
            IPostOpinionService postOpinionService, 
            IUserOpinionService userOpinionService, 
            IPostService postService, 
            IUserReportService userReportService, 
            IPostReportService postReportService)
        {
            _userService = userService;
            _postOpinionService = postOpinionService;
            _userOpinionService = userOpinionService;
            _postService = postService;
            _userReportService = userReportService;
            _postReportService = postReportService;
        }

        public Task ValidateAccountOwnership(Guid userId, Guid accountId)
        {
            return userId == accountId ? Task.CompletedTask  : throw new  AccessForbiddenException("ValidateAccountOwnership",userId.ToString(), "User id and Account id doesn't match");
        }

        public async Task ValidateOpinionOwnership(Guid userId, Guid opinionId)
        {
            var opinion = (await _userOpinionService.GetOpinionById(opinionId) ?? await _postOpinionService.GetOpinionById(opinionId)) ?? throw new EntityNotFoundException("Opinion");
            if(opinion.authorId != userId.ToString() ) throw new AccessForbiddenException("ValidateOpinionOwnerShip", userId.ToString(), "AuthorId and userId mismatch");

        }

        public async Task ValidatePostOwnership(Guid userId, Guid postId)
        {
            var post = await _postService.GetPostById(postId,userId) ?? throw new AccessForbiddenException("ValidatePostOwnership", userId.ToString(), "User haven't an access to this post, or post doesn't exists");
            if (post.OwnerId != userId.ToString()) throw new AccessForbiddenException("ValidatePostOwnership", userId.ToString(), "OwnerId and userId mismatch");

        }

        public async Task ValidateReportOwnership(Guid userId, Guid reportId)
        {
            var report = await  _userReportService.GetReportById(reportId) ??  await _postReportService.GetReportById(reportId) ?? throw new EntityNotFoundException("Report");
            if(report.AuthorId != userId) throw new AccessForbiddenException("ValidateReportOwnership", userId.ToString(), "AuthorId and userId mismatch");
        }
    }
}
