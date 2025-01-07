using Application.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class GmailService : IMailService
    {
        public Task SendConfiramationMail(string UserEmail)
        {
            throw new NotImplementedException();
        }

        public Task SendMail(string RecieverEmail, string subject, string body)
        {
            throw new NotImplementedException();
        }

        public Task SendPasswordResetMail(string UserEmail)
        {
            throw new NotImplementedException();
        }
    }
}
