using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces
{
    public interface IMailService
    {
       
        Task SendMail(string RecieverEmail, string subject, string body);
        Task SendConfiramationMail(string UserEmail);
        Task SendPasswordResetMail(string UserEmail);
    }
}
