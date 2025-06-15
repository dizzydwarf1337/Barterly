using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Posts.Events.PostApprovedEvent
{
    public class PostApprovedEvent : INotification
    {
        public required string postId { get; set; }
    }
}
