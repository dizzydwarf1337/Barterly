using Application.Interfaces;
using Domain.Interfaces.Commands.Post;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Posts.Events.PostVisitedEvent
{
    public class PostVisitedEventHandler : INotificationHandler<PostVisitedEvent>
    {
        private readonly IPostCommandRepository _postCommandRepository;
        private readonly IVisitedPostCommandRepository _visitedPostCommandRepository;

        public PostVisitedEventHandler(IPostCommandRepository postCommandRepository, IVisitedPostCommandRepository visitedPostCommandRepository)
        {
            _postCommandRepository = postCommandRepository;
            _visitedPostCommandRepository = visitedPostCommandRepository;
        }

        public async Task Handle(PostVisitedEvent notification, CancellationToken cancellationToken)
        {

            if(notification.UserId != null)
            {
                await _visitedPostCommandRepository.VisitPost(notification.PostId, Guid.Parse(notification.UserId));  
            }
            else
            {
                await _postCommandRepository.IncreasePostView(notification.PostId);

            }

        }
    }
}
