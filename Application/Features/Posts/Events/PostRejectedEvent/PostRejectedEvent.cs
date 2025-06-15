using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Posts.Events.PostRejectedEvent
{
    public class PostRejectedEvent : INotification
    {
        public required string postId { get; set; }
        public required string reason { get; set; }
    }
}
