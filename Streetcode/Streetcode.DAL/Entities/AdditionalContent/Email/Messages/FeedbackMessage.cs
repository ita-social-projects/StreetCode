using MimeKit;
using Streetcode.DAL.Entities.AdditionalContent.Email.Messages.Base;

namespace Streetcode.DAL.Entities.AdditionalContent.Email.Messages;

public class FeedbackMessage : Message
{
    public FeedbackMessage(IEnumerable<string> to, string from, string source, string subject, string content) : base(to, from)
    {
        Source = source;
        Subject = subject;
        Content = content;
        From = from;
    }

    public string Source { get; set; }
    public string Subject { get; set; }
    public string Content { get; set; }
    public string From { get; set; }

    public override MimeMessage ToMimeMessage(EmailConfiguration emailConfig)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("Test", emailConfig.From));
        emailMessage.To.AddRange(To);

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody =
                "<h2 style='color:black;'>" +
                $"Звідки: {Source} <br>" +
                $"Від: {From} <br>" +
                $"Текст: {Content}" +
                "</h2>"
        };

        emailMessage.Body = bodyBuilder.ToMessageBody();
        return emailMessage;
    }
}