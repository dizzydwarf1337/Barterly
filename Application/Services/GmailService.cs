using Application.Interfaces;
using Domain.Enums.Common;
using Domain.Exceptions.BusinessExceptions;
using Domain.Exceptions.ExternalServicesExceptions;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace Application.Services
{
    public class GmailService : IMailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _senderEmail;
        private readonly string _senderPassword;
        private readonly string _senderName;
        private readonly ITokenService _tokenService;
        private readonly ILogService _logService;
        public GmailService(IConfiguration configuration, ITokenService tokenService, ILogService logService)
        {
            var emailConfig = configuration.GetSection("EmailSettings");
            _smtpServer = emailConfig["SmtpServer"] ?? throw new ConfigException("Smtp Server error");
            _smtpPort = int.Parse(emailConfig["SmtpPort"] ?? throw new ConfigException("Smtp Port error")) ;
            _senderEmail = emailConfig["SenderEmail"] ?? throw new ConfigException("Sender email error");
            _senderPassword = emailConfig["SenderPassword"] ?? throw new ConfigException("Sender Password error");
            _senderName = emailConfig["SenderName"] ?? throw new ConfigException("Sender name error");
            _tokenService = tokenService;
            _logService = logService;
        }

        public async Task SendMail(string receiverEmail, string subject, string body)
        {
            using var smtp = new SmtpClient(_smtpServer, _smtpPort)
            {
                Credentials = new NetworkCredential(_senderEmail, _senderPassword),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_senderEmail, _senderName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(receiverEmail);

            await smtp.SendMailAsync(mailMessage);
        }

        public async Task<bool> SendConfiramationMail(string userEmail)
        {
            try
            {
                var token = await _tokenService.GenerateEmailConfirmationToken(userEmail);
                var confirmationLink = $"http://localhost:3000/confirm?email={userEmail}&token={token}";
                var body = $"<h3>Confirm your email address</h3><p>Press <a href='{confirmationLink}'>here</a>, to confirm</p>";
                await SendMail(userEmail, "Email-confirmation", body);
                return true;
            }
            catch (Exception ex) 
            {
                await _logService.CreateLogAsync($"Error sending email to {userEmail}: {ex.Message}", LogType.Error, null, Guid.Empty, Guid.Empty);
                throw new ExternalServiceException($"Error sending email to {userEmail}: {ex.Message}");
            }
        }


        public async Task SendPasswordResetMail(string userEmail)
        {
            var token = await _tokenService.GeneratePasswordResetToken(userEmail);
            var resetLink = $"https://localhost:3000/reset-password?email={userEmail}&token={token}";
            var body = $"<h3>Reset your password</h3><p>Press <a href='{resetLink}'>here</a>, to reset, if it wasn't you, just ignore this message</p>";
            await SendMail(userEmail, "Reset-Password", body);
        }
    }
}
