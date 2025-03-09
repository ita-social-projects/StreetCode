using MimeKit;
using Streetcode.BLL.Constants.MessageData;
using Streetcode.BLL.Models.Email.Messages.Base;

namespace Streetcode.BLL.Models.Email.Messages
{
    public class DeleteConfirmationMessageData : MessageData
    {
        public string From { get; set; } = string.Empty;

        public override MimeMessage ToMimeMessage()
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("", From));
            emailMessage.To.AddRange(To.Select(x => new MailboxAddress(string.Empty, x)));
            emailMessage.Subject = MessageDataConstants.DeleteConfirmationTitle;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $"""
                            <p>Привіт,</p>
                            <p>Ваш акаунт було успішно видалено. Ви можете створити новий акаунт в будь-який час.</p>
                            <p>З найкращими побажаннями,</p>
                            <p>Команда Historycode</p>
                            """,
            };

            emailMessage.Body = bodyBuilder.ToMessageBody();
            return emailMessage;
        }
    }
}