using MyPortfolio.Core.Interfaces;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;

namespace MyPortfolio.Business.Services
{
    public class FileEmailService : IEmailService
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<FileEmailService> _logger;

        public FileEmailService(IWebHostEnvironment env, ILogger<FileEmailService> logger)
        {
            _env = env;
            _logger = logger;
        }

        public Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var emailDirectory = Path.Combine(_env.ContentRootPath, "emails");
            if (!Directory.Exists(emailDirectory))
            {
                Directory.CreateDirectory(emailDirectory);
            }

            var filePath = Path.Combine(emailDirectory, $"{toEmail}-{Guid.NewGuid()}.txt");

            var emailContent = $"To: {toEmail}\nSubject: {subject}\n\n{message}";

            File.WriteAllText(filePath, emailContent);

            _logger.LogInformation($"Email sent to {toEmail} and saved to {filePath}");

            return Task.CompletedTask;
        }
    }
}