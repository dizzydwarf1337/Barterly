using MediatR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Events.UserCreated
{
    public class UserCreatedEvent : INotification
    {
        public UserCreatedEvent(string email, string firstName,string lastName,Guid userId)
        {
            UserId = userId;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
        }
        public Guid UserId { get; set; }
        public string Email {  get; set; }
        public string FirstName {  get; set; }
        public string LastName { get; set; }

    }
}
