namespace Application.Interfaces;

public interface IMailService
{
    Task SendMail(string RecieverEmail, string subject, string body);
    Task<bool> SendConfiramationMail(string UserEmail, CancellationToken token);
    Task SendPasswordResetMail(string UserEmail);
}