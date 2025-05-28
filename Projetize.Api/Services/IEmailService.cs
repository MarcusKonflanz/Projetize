using SendGrid;
using SendGrid.Helpers.Mail;
using System.Runtime.InteropServices;

namespace Projetize.Api.Services
{

    public interface IEmailService
    {
        Task SendConfirmationEmailAsync(string email, string toName, string token);
        Task SendPasswordResetEmailAsync(string email, string toName, string token);
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
            try
            {
                var apiKey = _configuration["SendGrid:ApiKey"];
                var fromEmail = _configuration["SendGrid:FromEmail"];
                var fromName = _configuration["SendGrid:FromName"];

                var client = new SendGridClient(apiKey);
                var from = new EmailAddress(fromEmail, fromName);
                var subject = "Confirmação de e-mail - Projetize";
                var to = new EmailAddress(toEmail, toName);
                var plainTextContent = $"Seu código de confirmação chegou!";
                var htmlContent = $@"< !DOCTYPE html>
                                <html lang=\""pt - br\"">
                                            < head >
                                                < meta charset =\""UTF-8\"">
                                                < meta name =\""viewport\"" content=\""width=device-width, initial-scale=1.0\"">
                                                < title > Email Estilizado </ title >
                                            </ head >
                                            < body style =\""font-family: Arial, sans-serif; margin: 0; padding: 0; background-color: #f9f9f9;\"">
                                                < div style =\""background-color: #007bff; color: white; padding: 20px; text-align: center;\"">
                                                    < h1 > Bem - vindo ao<strong> Projetize</ strong > !</ h1 >    
                                                </ div >
                                                < div style =\""padding: 20px; background-color: white; max-width: 600px; margin: 0 auto; border-radius: 8px;\"">
                                                    < p style =\""font-size: 16px; line-height: 1.5;\"">Olá, <strong>{{toName}}</strong>,</p>
                                                    < p style =\""font-size: 16px; line-height: 1.5;\"">Obrigado por se inscrever em nosso serviço. Estamos empolgados em tê-lo conosco.</p>
                                                    < p style =\""font-size: 16px; line-height: 1.5;\"">Aqui está o seu código de verificação:</p>
                                                    < h2 style =\""color: #007bff; text-align: center;\"">{{token.ToLower()}}</h2> <!-- Código de 6 dígitos gerado -->
                                                    < p style =\""font-size: 14px; color: #777; text-align: center; margin-top: 20px;\"">
                                            Se você não fez essa solicitação, ignore este e-mail.
                                        </ p >
                                    </ div >
                                    < div style =\""text-align: center; padding: 20px; font-size: 12px; color: #777;\"">
                                        < p > &copy; 2025 Sua Empresa. Todos os direitos reservados.</ p >
                                    </ div >
                                </ body >
                                </ html > ";

                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao enviar o e-mail: {ex.Message}");
            }
        }

        public async Task SendPasswordResetEmailAsync(string toEmail, string toName, string token)
        {
            var apiKey = _configuration["SendGrid:ApiKey"];
            var fromEmail = _configuration["SendGrid:FromEmail"];
            var fromName = _configuration["SendGrid:FromName"];

            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(fromEmail, fromName);
            var subject = "Recuperação de senha - Projetize";
            var to = new EmailAddress(toEmail, toName);
            var plainTextContent = $"Seu código de recuperação chegou!";
            var htmlContent = $"<strong>Você solicitou a recuperação de senha no Projetize. Use o seguinte código para redefinir: {token}. Este código expira em 15 minutos. </strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
