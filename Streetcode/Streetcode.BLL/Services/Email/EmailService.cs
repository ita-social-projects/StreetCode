using MailKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using Streetcode.BLL.Interfaces.Email;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Entities.AdditionalContent.Email;
using Streetcode.DAL.Entities.AdditionalContent.Email.Messages.Base;

namespace Streetcode.BLL.Services.Email;

public class EmailService : IEmailService
{
    private readonly EmailConfiguration _emailConfig;
    private readonly ILoggerService _logger;

    public EmailService(IOptions<EmailConfiguration> emailConfig, ILoggerService logger)
    {
        _emailConfig = emailConfig.Value;
        _logger = logger;
    }

    public async Task<bool> SendEmailAsync(Message message)
    {
        return await SendAsync(message.ToMimeMessage(_emailConfig));
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