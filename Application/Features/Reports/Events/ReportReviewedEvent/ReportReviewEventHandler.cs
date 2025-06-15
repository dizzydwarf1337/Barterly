using Application.Interfaces;
using Domain.Entities.Posts;
using Domain.Entities.Users;
using Domain.Enums.Common;
using Domain.Interfaces.Commands.User;
using Domain.Interfaces.Queries.Post;
using Domain.Interfaces.Queries.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Reports.Events.ReportReviewdEvent
{
    public class ReportReviewEventHandler : INotificationHandler<ReportReviewedEvent>
    {
        private readonly INotificationCommandRepository _notificationCommandRepository;
        private readonly IUserQueryRepository _userQueryRepository;
        private readonly IPostQueryRepository _postQueryRepository;

        public ReportReviewEventHandler(INotificationCommandRepository notificationCommandRepository, IUserQueryRepository userQueryRepository, IPostQueryRepository postQueryRepository)
        {
            _notificationCommandRepository = notificationCommandRepository;
            _userQueryRepository = userQueryRepository;
            _postQueryRepository = postQueryRepository;
        }

        public async Task Handle(ReportReviewedEvent notification, CancellationToken cancellationToken)
        {
            Notification userNotification = default!;
            if (notification.Report is ReportUser userReport)
            {
                var user = await _userQueryRepository.GetUserAsync(userReport.ReportedUserId);
                userNotification = new Notification
                {
                    Title = "Your report has been reviewed",
                    UserId = userReport.AuthorId,
                    Message = userReport.Status == ReportStatusType.Approved ? $"Your report on {user.FirstName} {user.LastName}. Our moderators will take the appropriate action. Thank you for your contribution."
                                                    : $"Your report on {user.FirstName} {user.LastName} has been reviewed, but we found no violations. Thank you for your vigilance."

                };
            }
            else if (notification.Report is ReportPost reportPost)
            {
                var post = await _postQueryRepository.GetPostById(reportPost.ReportedPostId);
                userNotification = new Notification
                {
                    Title = "Your report has been reviewed",
                    UserId = reportPost.AuthorId,
                    Message = reportPost.Status == ReportStatusType.Approved ? $"Your report on {post.Title}. Our moderators will take the appropriate action. Thank you for your contribution."
                                                                            : $"Your report on {post.Title} has been reviewed, but we found no violations. Thank you for your vigilance."
                };
            }
            await _notificationCommandRepository.CreateNotificationAsync(userNotification);
        }
    }
}
