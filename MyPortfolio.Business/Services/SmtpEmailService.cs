using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using MyPortfolio.Core.Interfaces;
using System.Net.Mail;
using System.Threading.Tasks;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace MyPortfolio.Business.Services
{
    public class SmtpEmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<SmtpEmailService> _logger;

        public SmtpEmailService(IConfiguration config, ILogger<SmtpEmailService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var emailSettings = _config.GetSection("EmailSettings");
            var host = emailSettings["SmtpHost"];
            var port = int.Parse(emailSettings["SmtpPort"]);
            var fromEmail = emailSettings["FromEmail"];
            var password = emailSettings["SmtpPass"]; 

            if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(fromEmail) || string.IsNullOrEmpty(password))
            {
                _logger.LogError("Email settings (SmtpHost, FromEmail, SmtpPass)");
                return;
            }

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(fromEmail));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = message;
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            try
            {
                await smtp.ConnectAsync(host, port, SecureSocketOptions.StartTls);
                // Autentifikasiya edirik (Gmail App Password ilə)
                await smtp.AuthenticateAsync(fromEmail, password);
                // Məktubu göndəririk
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
                _logger.LogInformation($"E-mail sent successfully!: {toEmail}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"E-mail can't sent: {toEmail}");
            }
        }
    }
}
