using MimeKit;
using Streetcode.BLL.Constants.MessageData;
using Streetcode.BLL.Models.Email.Messages.Base;

namespace Streetcode.BLL.Models.Email.Messages;

public class ForgotPasswordMessageData : MessageData
{
    public string From { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string CurrentDomain { get; set; } = string.Empty;

    public override MimeMessage ToMimeMessage()
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("", From));
        emailMessage.To.AddRange(To.Select(x => new MailboxAddress(string.Empty, x)));
        emailMessage.Subject = MessageDataConstants.ForgotPasswordTitle;

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = $"""
                      <p>Привіт,</p>
                      <p>Ви отримали цей лист, оскільки ми отримали запит на відновлення паролю для вашого облікового запису. Щоб створити новий пароль, будь ласка, натисніть кнопку нижче:</p>
                      <a href="{CurrentDomain}/forgot-password-reset?token={Token}&username={Username}" class="btn">Відновити пароль</a>
                      <p>Якщо ви не робили цього запиту, просто проігноруйте цей лист.</p>
                      <p>З найкращими побажаннями,<br>Команда підтримки</p>
                      """,
        };

        emailMessage.Body = bodyBuilder.ToMessageBody();
        return emailMessage;
    }
}