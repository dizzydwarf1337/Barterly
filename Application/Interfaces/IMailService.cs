namespace Application.Interfaces
{
    public interface IMailService
    {

        Task SendMail(string RecieverEmail, string subject, string body);
        Task<bool> SendConfiramationMail(string UserEmail);
        Task SendPasswordResetMail(string UserEmail);
    }
}
