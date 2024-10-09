using MailKit;
using MailKit.Net.Smtp;
using MimeKit;
using Streetcode.BLL.Interfaces.Email;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Entities.AdditionalContent.Email;

namespace Streetcode.BLL.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfig;
        private readonly ILoggerService _logger;

        public EmailService(EmailConfiguration emailConfig, ILoggerService logger)
        {
            _emailConfig = emailConfig;
            _logger = logger;
        }

        public async Task<bool> SendEmailAsync(Message message)
        {
            var mailMessage = CreateEmailMessage(message);

            return await SendAsync(mailMessage);
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("", _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            var bodyBuilder = new BodyBuilder
            {
              HtmlBody =
                "<h2 style='color:black;'>" +
                $"Звідки: {message.Source} <br>" +
                $"Від: {message.From} <br>" +
                $"Текст: {message.Content}" +
                "</h2>"
            };

            emailMessage.Body = bodyBuilder.ToMessageBody();
            return emailMessage;
        }

        private async Task<bool> SendAsync(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient(new ProtocolLogger(Console.OpenStandardOutput())))
            {
                try
                {
                    await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);

                    await client.SendAsync(mailMessage);
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex.Message);
                    return false;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                 }
            }
        }
    }
}
