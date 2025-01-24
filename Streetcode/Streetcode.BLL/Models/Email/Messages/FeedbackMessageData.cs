using MimeKit;
using Streetcode.BLL.Models.Email.Messages.Base;

namespace Streetcode.BLL.Models.Email.Messages;

public class FeedbackMessageData : MessageData
{
    public string Source { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string From { get; set; } = string.Empty;

    public override MimeMessage ToMimeMessage()
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("", From));
        emailMessage.To.AddRange(To.Select(x => new MailboxAddress(string.Empty, x)));
        emailMessage.Subject = "Відгук";

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