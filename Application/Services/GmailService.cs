using System.Net;
using System.Net.Mail;
using Application.Interfaces;
using Domain.Enums.Common;
using Domain.Exceptions.BusinessExceptions;
using Domain.Exceptions.ExternalServicesExceptions;
using Microsoft.Extensions.Configuration;

namespace Application.Services;

public class GmailService : IMailService
{
    private readonly ILogService _logService;
    private readonly string _senderEmail;
    private readonly string _senderName;
    private readonly string _senderPassword;
    private readonly int _smtpPort;
    private readonly string _smtpServer;
    private readonly ITokenService _tokenService;

    public GmailService(IConfiguration configuration, ITokenService tokenService, ILogService logService)
    {
        var emailConfig = configuration.GetSection("EmailSettings");
        _smtpServer = emailConfig["SmtpServer"] ?? throw new ConfigException("Smtp Server error");
        _smtpPort = int.Parse(emailConfig["SmtpPort"] ?? throw new ConfigException("Smtp Port error"));
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

    public async Task<bool> SendConfiramationMail(string userEmail, CancellationToken cancToken)
    {
        try
        {
            var token = await _tokenService.GenerateEmailConfirmationToken(userEmail, cancToken);
            var confirmationLink = $"http://localhost:3000/confirm?email={userEmail}&token={token}";
            var body = $@"
                <body>
                    <div style='font-family: Arial, sans-serif; padding: 20px; background-color: #f9f9f9; color: #333;'>
                        <div style='max-width: 600px; margin: auto; background-color: #ffffff; border-radius: 8px; padding: 30px; box-shadow: 0 2px 8px rgba(0,0,0,0.05);'>
                            <h2 style='color: #2c3e50;'>Confirm Your Email Address</h2>
                            <p style='font-size: 16px; line-height: 1.6;'>Thank you for registering with us!</p>
                            <p style='font-size: 16px; line-height: 1.6;'>To complete your registration, please confirm your email address by clicking the button below:</p>
                            <div style='text-align: center; margin: 30px 0;'>
                                <a href='{confirmationLink}' style='background-color: #4CAF50; color: white; padding: 12px 25px; text-decoration: none; border-radius: 4px; font-size: 16px;'>Confirm Email</a>
                            </div>
                            <p style='font-size: 14px; color: #777;'>If you didn’t request this, you can safely ignore this email.</p>
                            <p style='font-size: 14px; color: #777;'>— The Barterly Team</p>
                        </div>
                    </div>
                </body>";

            await SendMail(userEmail, "Email-confirmation", body);
            return true;
        }
        catch (Exception ex)
        {
            await _logService.CreateLogAsync($"Error sending email to {userEmail}: {ex.Message}", cancToken,
                LogType.Error, null, Guid.Empty, Guid.Empty);
            throw new ExternalServiceException($"Error sending email to {userEmail}: {ex.Message}");
        }
    }


    public async Task SendPasswordResetMail(string userEmail)
    {
        var token = await _tokenService.GeneratePasswordResetToken(userEmail);
        var resetLink = $"https://localhost:3000/reset-password?email={userEmail}&token={token}";
        var body =
            $"<h3>Reset your password</h3><p>Press <a href='{resetLink}'>here</a>, to reset, if it wasn't you, just ignore this message</p>";
        await SendMail(userEmail, "Reset-Password", body);
    }
}