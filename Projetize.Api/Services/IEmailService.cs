using SendGrid;
using SendGrid.Helpers.Mail;

namespace Projetize.Api.Services
{

    public interface IEmailService
    {
        Task SendConfirmationEmailAsync(string email, string toName, string token);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendConfirmationEmailAsync(string toEmail, string toName, string token)
        {
            var apiKey = _configuration["SendGrid:ApiKey"];
            var fromEmail = _configuration["SendGrid:FromEmail"];
            var fromName = _configuration["SendGrid:FromName"];

            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(fromEmail, fromName);
            var subject = "Confirmação de e-mail - Projetize";
            var to = new EmailAddress(toEmail, toName);
            var plainTextContent = $"Seu código de confirmação é: {token}";
            var htmlContent = $"<strong>Seu código de confirmação é: {token}</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
