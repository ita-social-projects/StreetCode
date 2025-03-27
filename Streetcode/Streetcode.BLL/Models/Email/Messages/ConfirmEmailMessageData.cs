using MimeKit;
using Streetcode.BLL.Constants.MessageData;
using Streetcode.BLL.Models.Email.Messages.Base;

namespace Streetcode.BLL.Models.Email.Messages
{
    public class ConfirmEmailMessageData : MessageData
    {
        public string From { get; set; } = string.Empty;
        public string Token { get; set; }
        public string Username { get; set; } = string.Empty;
        public string CurrentDomain { get; set; } = string.Empty;

        public override MimeMessage ToMimeMessage()
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("", From));
            emailMessage.To.AddRange(To.Select(x => new MailboxAddress(string.Empty, x)));
            emailMessage.Subject = MessageDataConstants.EmailConfirmationTitle;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $"""
                            <p>Дякуємо за реєстрацію!</p>
                            <p>Будь ласка, підтвердьте свою електронну адресу, натиснувши на посилання нижче:</p>
                            <p>[<a href="{CurrentDomain}/confirm-email?token={Token}&username={Username}" class="btn">Підтвердити електронну адресу</a>]</p>
                            <p>Якщо ви не створювали цей акаунт, просто ігноруйте цей лист.</p>
                            <p>З найкращими побажаннями,<br>Команда Historycode</p>
                            """,
            };

            emailMessage.Body = bodyBuilder.ToMessageBody();
            return emailMessage;
        }
    }
}