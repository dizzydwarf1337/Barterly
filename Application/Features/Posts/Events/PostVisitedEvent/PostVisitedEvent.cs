using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Posts.Events.PostVisitedEvent
{
    public class PostVisitedEvent : INotification
    {
        public Guid PostId {  get; set; }
        public string? UserId { get; set; }
    }
}
