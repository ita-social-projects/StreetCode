using MimeKit;
using Streetcode.DAL.Entities.AdditionalContent.Email.Messages.Base;

namespace Streetcode.DAL.Entities.AdditionalContent.Email.Messages;

public class ForgotPasswordMessage : Message
{
    public ForgotPasswordMessage(IEnumerable<string> to, string from, string token, string username) : base(to, from)
    {
        Token = token;
        Username = username;
    }

    public string Token { get; set; }
    public string Username { get; set; }
    public override MimeMessage ToMimeMessage(EmailConfiguration emailConfig)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("Test", emailConfig.From));
        emailMessage.To.AddRange(To);

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody =
                "<h2 style='color:black;'>" +
                $"Від: {Token} {Username} <br>" +
                "</h2>"
        };

        emailMessage.Body = bodyBuilder.ToMessageBody();
        return emailMessage;
    }
}