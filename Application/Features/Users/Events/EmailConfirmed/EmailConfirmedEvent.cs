using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Events.EmailConfirmed
{
    public class EmailConfirmedEvent : INotification
    {
        public string Email { get; set; }
    }
}
